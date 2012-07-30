if (typeof jq == "undefined")
    jq = jQuery.noConflict();

//ini
jq(function() {
    InitLuckLotteryNumber_CJDLT();         //初始化选择第一个
    jq("#tab li").click(function() {
        number = "";
        jq(this).addClass("li_hover");
        jq(this).siblings().removeClass("li_hover");

    });
    jq(".main_table tr:even td").css({ "height": "78px" });
    jq(".main_table img").click(
        function() {
    jq(".main_table img").not(this).height(70).width(72);            
            jq(this).height(jq(this).height() + 5);
            jq(this).width(jq(this).width() + 5);            
        });
});

//*************************************************************
    


//玩法
var lastXYJX = null;
function ClickXYJX(obj, type) {
    number = "";
    jq("#xxk>div").css("display", "none");
    jq("#" + obj).css("display", "block");
    
    document.getElementById("HidType").value = type;
    
    document.getElementById("HidLuckNumber").value = "";
    var tds = document.getElementById("tdLuckNumber").getElementsByTagName("td");
    for (var i = 0; i < tds.length; i++) {
        if (tds[i].id.indexOf("tdLuckNumber") > -1) {
            tds[i].innerHTML = "-";
        }
    }

    return false;
}


var moving;
var number;
var isMove = true;
function GetLuckNumber() {
    lotteryID = jq("#HidLotteryID").val();
    lotteryName = jq("#HidlotteryName").val();

   setHidControl(lotteryID);                       //得到彩票的信息
    
    var type = document.getElementById("HidType").value;
    var name = "";
    switch (type) {
        case "1":
            name = document.getElementById("ddlXiZuo").value;
            break;
        case "2":
            name = document.getElementById("ddlSX").value;
            break;
        case "3":
            var date = document.getElementById("tbDate").value;

            if (date == "") {
                msg("请输入出生日期！");
                document.getElementById("tbDate").focus();
                return false;
            }

            name = date;
            break;
        case "4":
            name = document.getElementById("tbName").value;

            if (name == "" || name == "支持中英文") {
                msg("请输入姓名！");
                document.getElementById("tbName").focus();
                return false;
            } break;
    }

    var v = Buy_Luck_Buy.GenerateLuckLotteryNumber(lotteryID, type, name, lotteryName).value;
    
    if (v.split("|").length < 2) {
        msg(v);
        document.getElementById("tbDate").value = "";
    } else {
    
        document.getElementById("HidLuckNumber").value = v.split("|")[0];
        document.getElementById("tb_LotteryNumber").value = document.getElementById("HidLuckNumber").value + "\n";

        number = v.split("|")[1].split(" ");

        moving = setInterval("BallMoving()", 50);

        setTimeout("BindLuckNumber()", 1000);
        isMove = true;
    }

}

//球滚动
function BallMoving() {
    if (isMove) {
        for (var i = 0; i < number.length; i++) {
            document.getElementById("tdLuckNumber" + i.toString()).innerHTML = number[GetRandNumber(number.length - 1)];
        }
    }
}

function BindLuckNumber() {
    clearInterval(moving);
    isMove = false;
    for (var i = 0; i < number.length; i++) {
        document.getElementById("tdLuckNumber" + i.toString()).innerHTML = number[i];
    }
}


//选择值
function setoption(obj, v) {
    jq("#ddlSX").val(v);
    jq("#ddlXiZuo").val(v);
}





//生成数字跳动的匡
//cjdlt
function InitLuckLotteryNumber_CJDLT() {
    var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">";
    sb += "<tr>"
    sb += "<td height=\"22\" align=\"center\">"
    sb += "&nbsp;</td>";

    for (var i = 0; i < 5; i++) {
        sb += "<td align=\"center\">"
        sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
        sb += "<tr>"
        sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
        sb += "-"
        sb += "</td></tr></table></td>";
    }
    for (var i = 0; i < 2; i++) {
        sb += "<td align=\"center\">"
        sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
        sb += "<tr>"
        sb += "<td height=\"22\" align=\"center\" class=\"blue12\" id='tdLuckNumber" + (5 + i).toString() + "'>"
        sb += "-"
        sb += "</td></tr></table></td>";
    }

    sb += "<td>&nbsp;</td>"
    sb += "</tr>"
    sb += "</table>";

    jq("#tdLuckNumber").html(sb);
    jq("#HidLotteryID").val("39");
    jq("#HidlotteryName").val("cjdlt");

    
}


    //排列3
function InitLuckLotteryNumber_PL3() {
    var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
    sb += "<tr>"
    sb += "<td height=\"22\" align=\"center\">"
    sb += "&nbsp;</td>";

    for (var i = 0; i < 3; i++) {
        sb += "<td align=\"center\">"
        sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
        sb += "<tr>"
        sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
        sb += "-"
        sb += "</td></tr></table></td>";
    }

    sb += "<td>&nbsp;</td>"
    sb += "</tr>"
    sb += "</table>";

    jq("#tdLuckNumber").html(sb);
    jq("#HidLotteryID").val("63");
    jq("#HidlotteryName").val("pl3");

    
}

  //双色球
function InitLuckLotteryNumber_SSQ() {

    var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
    sb += "<tr>"
    sb += "<td height=\"22\" align=\"center\">"
    sb += "&nbsp;</td>";

    for (var i = 0; i < 6; i++) {
        sb += "<td align=\"center\">"
        sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
        sb += "<tr>"
        sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
        sb += "-"
        sb += "</td></tr></table></td>";
    }
    for (var i = 0; i < 1; i++) {
        sb += "<td align=\"center\">"
        sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
        sb += "<tr>"
        sb += "<td height=\"22\" align=\"center\" class=\"blue12\" id='tdLuckNumber" + (6 + i).toString() + "'>"
        sb += "-"
        sb += "</td></tr></table></td>";
    }

    sb += "<td>&nbsp;</td>"
    sb += "</tr>"
    sb += "</table>";

    jq("#tdLuckNumber").html(sb);
    jq("#HidLotteryID").val("5");
    jq("#HidlotteryName").val("ssq");

    
}


    //3D
    function  InitLuckLotteryNumber_3D()
    {

        var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
            sb += "<tr>"
            sb += "<td height=\"22\" align=\"center\">"
            sb += "&nbsp;</td>";

        for (var i = 0; i < 3; i++)
        {
            sb += "<td align=\"center\">"
                sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
                sb += "<tr>"
                sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
                sb += "-"
                sb += "</td></tr></table></td>";
        }
    
        sb += "<td>&nbsp;</td>"
            sb += "</tr>"
            sb += "</table>";

            jq("#tdLuckNumber").html(sb);
            jq("#HidLotteryID").val("6");
            jq("#HidlotteryName").val("3d");

            
    }

   //7乐彩
    function InitLuckLotteryNumber_QLC()
    {
       var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
            sb += "<tr>"
            sb += "<td height=\"22\" align=\"center\">"
            sb += "&nbsp;</td>";

        for (var i = 0; i < 7; i++)
        {
            sb += "<td align=\"center\">"
                sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
                sb += "<tr>"
                sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
                sb += "-"
                sb += "</td></tr></table></td>";
        }

        sb += "<td>&nbsp;</td>"
            sb += "</tr>"
            sb += "</table>";

            jq("#tdLuckNumber").html(sb);
            jq("#HidLotteryID").val("13");
            jq("#HidlotteryName").val("qlc");

            
        }
    
        //排列5
    function InitLuckLotteryNumber_PL5()
    {
        var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
            sb += "<tr>"
            sb += "<td height=\"22\" align=\"center\">"
            sb += "&nbsp;</td>";

        for (var i = 0; i < 5; i++)
        {
            sb += "<td align=\"center\">"
                sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
                sb += "<tr>"
                sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
                sb += "-"
                sb += "</td></tr></table></td>";
        }

        sb += "<td>&nbsp;</td>"
            sb += "</tr>"
            sb += "</table>";

            jq("#tdLuckNumber").html(sb);
            jq("#HidLotteryID").val("64");
            jq("#HidlotteryName").val("pl5");

            
        }
    
        //七星彩
    function  InitLuckLotteryNumber_QXC()
    {       
        var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
            sb += "<tr>"
            sb += "<td height=\"22\" align=\"center\">"
            sb += "&nbsp;</td>";

        for (var i = 0; i < 7; i++)
        {
            sb += "<td align=\"center\">"
                sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
                sb += "<tr>"
                sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
                sb += "-"
                sb += "</td></tr></table></td>";
        }

        sb += "<td>&nbsp;</td>"
            sb += "</tr>"
            sb += "</table>";

            jq("#tdLuckNumber").html(sb);
            jq("#HidLotteryID").val("3");
            jq("#HidlotteryName").val("qxc");

            
        }
    
        //22X5
    function InitLuckLotteryNumber_22X5()
    {        

        var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
            sb += "<tr>"
            sb += "<td height=\"22\" align=\"center\">"
            sb += "&nbsp;</td>";

        for (var i = 0; i < 5; i++)
        {
            sb += "<td align=\"center\">"
                sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
                sb += "<tr>"
                sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
                sb += "-"
                sb += "</td></tr></table></td>";
        }

        sb += "<td>&nbsp;</td>"
            sb += "</tr>"
            sb += "</table>";

            jq("#tdLuckNumber").html(sb);
            jq("#HidLotteryID").val("9");
            jq("#HidlotteryName").val("22x5");

            
    }


 //31X7
    function InitLuckLotteryNumber_31X7()
    {
        var sb = "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 10px;\">"
            sb += "<tr>"
            sb += "<td height=\"22\" align=\"center\">"
            sb += "&nbsp;</td>";

        for (var i = 0; i < 7; i++)
        {
            sb += "<td align=\"center\">"
                sb += "<table width=\"22\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" background=\"../Home/Room/Images/ssq_bg_td.jpg\">"
                sb += "<tr>"
                sb += "<td height=\"22\" align=\"center\" class=\"red12\" id='tdLuckNumber" + i.toString() + "'>"
                sb += "-"
                sb += "</td></tr></table></td>";
        }

        sb += "<td>&nbsp;</td>"
            sb += "</tr>"
            sb += "</table>";

            jq("#tdLuckNumber").html(sb);
            jq("#HidLotteryID").val("65");
            jq("#HidlotteryName").val("31x7");            
    }


    //*************************给隐藏控件赋值**************************
    //彩票ID
    function setHidControl(id) {
        var o = Buy_Luck_Buy.returnHidValue(id);
        var arr = o.value.split('|');

        jq("#HidIsuseID").val(arr[0]);
        jq("#HidIsuseEndTime").val(arr[1]);
        jq("#tbPlayTypeID").val(arr[2]);
       
    }
    


    //************************************************************投注***************************

    function btn_OKClick() {        
        if (isMove) {       //数字是否在跳动
            return false;
        }
        if (number == "") {
            msg("请选择幸运数字!");
            return false;
        }
        if (jq("#HidIsuseID").val() == "-1") {
            msg("请选择幸运数字!");
            return false;
        }
        if (jq("#HidIsuseEndTime").val() == "-1") {
            msg("请选择幸运数字!");
            return false;
        }
        if (jq("#tbPlayTypeID").val() == "-1") {
            msg("请选择幸运数字!");
            return false;
        }
        if (jq("#HidIsuseID").val() == "") {
            msg("该期已经过期!");
            return false;
        }
        
        var multiple = 1; 	//倍数
        var SumNum = 1; 		//注数
        var Share = 1; 		//总份数
        var BuyShare = 1; 	//购买份数
        var AssureShare = 0; 	//保底份数
        var SumMoney = StrToInt($Id('HidPrice').value); 	//总金额
        var AssureMoney = 0; //保底的金额
        var BuyMoney = StrToFloat($Id('HidPrice').value); //购买金额
        var o_tb_Price = $Id('HidPrice').value;                     //单价
        var o_lab_ShareMoney = $Id('HidPrice').value;       //每份金额        
        var LotteryName = "";

        if ((SumMoney < o_tb_Price) || (SumMoney > 1000000)) {
            msg("单个方案的总金额必须在" + o_tb_Price + "元至 1000000 元之间。");
            return false;
        }

        var TipStr = '您要发起' + jq(".li_hover").text() + $Id("tbPlayTypeName").value + '方案，详细内容：\n\n';
        TipStr += "　　注　数：　" + SumNum + "\n";
        TipStr += "　　倍　数：　" + multiple + "\n";
        TipStr += "　　总金额：　" + SumMoney + " 元\n\n";
        TipStr += "　　总份数：　" + Share + " 份\n";
        TipStr += "　　每　份：　" + o_lab_ShareMoney + " 元\n\n";
        TipStr += "　　保　底：　" + AssureShare + " 份，" + AssureMoney + " 元\n";
        TipStr += "　　购　买：　" + BuyShare + " 份，" + BuyMoney + " 元\n\n";

        if (!confirm(TipStr + "按“确定”即表示您已阅读《" + LotteryName + "投注协议》并立即提交方案，确定要提交方案吗？"))
            return false;
        $Id("tb_hide_SumMoney").value = SumMoney;
        $Id("tb_hide_AssureMoney").value = AssureMoney;
        $Id("tb_hide_SumNum").value = SumNum;

        __doPostBack('btn_OK', '');

    }

