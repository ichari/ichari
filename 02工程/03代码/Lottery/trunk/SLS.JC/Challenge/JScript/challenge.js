window.$ = jQuery;
var o = ""; //赔率
var _buyState = true;
CheckedBg = function(td, trid) {
    // 注，防止单击 td  (区分单击的td 和 checkbox)
    $(td).children("input").each(function() {
        $(this).attr("checked", true);
    })

    // 控制 用户选择多场
    var state = true;
    $("#" + trid).children("td").each(function() {
        if (this.className == "sp_3") {
            state = false;
            o = "";
        }

    });

    if ($("input[type='checkbox']:checked").length > getGgtypeId() && state) {
        msg("竞彩擂台只能选择" + getGgtypeId() + "场比赛");
        $(td).children("input").each(function() {
            $(this).attr("checked", false);
        });
        td.className = "sp_bg";
        return;
    }


    if (td.className == "sp_bg") {
        // 取消
        $("#" + trid).children("td").each(function() {
            
            if (this.className = "tds") {
                this.className = "sp_bg";
            }

            $(this).children("input[type=checkbox]").attr("checked", false);
        });
    }

    if (td.className == "sp_bg") {
        $(td).children("input[type=checkbox]").eq(0).attr("checked", true);
        td.className = "sp_3";
    } else {
        $(td).children("input[type=checkbox]").eq(0).attr("checked", false);
        td.className = "sp_bg";
    }

    // 注数 和 场数
    $("#counts").text($("input[type='checkbox']:checked").length);

    $("#betCount").text(parseInt($("#counts").text()) == getGgtypeId() ? "1" : "0");

    if ($("#betCount").text() == "1") {
        var Content = ";[";
        var i = 0;

        $("input[type='checkbox']:checked").each(function() {

            var odds = $(this).attr("id");     //赔率
            o += "|" + odds;
            i++;
            Content = Content + $(this).attr("sid") + "(" + $(this).attr("rid") + ")|";

        });

        Content = Content.substring(0, Content.length - 1) + "]";
        o = o.substring(1);

        if (i == getGgtypeId()) {
            var os = o.split('|');
            var osSum = 1.0;
            for (var x = 0; x < os.length; x++) {
                osSum *= parseFloat(os[x]);
            }
            osSum *= 2;
            $("#GuessMoney").text(osSum.toFixed(2));
            $("#betMoney").show();

            $("#odds").attr("value", o);
            // 组成code  投注号码
            $("#codes").attr("value", $("#playid").attr("value") + Content + ";[" + $("#ggtypeid").attr("value") + $("#beishu").attr("value") + "]");
        }


    } else {
        $("#GuessMoney").text(0);
        $("#betMoney").hide();
        $("#odds").attr("value", "");
        $("#codes").attr("value", "");
        o = "";


    }
}

clearSelect = function() {
    $("#table_vs td").each(function() {
        
        if (this.className == "sp_3") {
           this.className = "sp_bg";
        }
        $(this).children("input").attr("checked", false);
    });
    $("#GuessMoney").text(0);
    $("#betMoney").hide();
    $("#odds").attr("value", "");
    $("#codes").attr("value", "");
    o = "";
    $("#counts").text("0");
    $("#betCount").text("0");
}
 
 
getGgtypeId = function() {
    var ggtypeid = $("#ggtypeid").attr("value");
    if (ggtypeid == "AA") {
        return 2;
    } else if (ggtypeid == "AB") {
        return 3;
    } else if (ggtypeid == "AC") {
        return 4;
    } else if (ggtypeid == "AD") {
        return 5;
    } else if (ggtypeid == "AQ") {
        return 6;
    }
    else if (ggtypeid == "BA") {
        return 6;
    }
    else if (ggtypeid == "BG") {
        return 6;
    }
}
dgBtnSubmit = function(subType) {
    if ($("input[type='checkbox']:checked").length < 2) {
        msg("请至少选择2场比赛，再进行投注");
        return;
    }
    if (!CreateLogin('')) {
        return;
    }
    else {

        var oddsMessage = (parseFloat($("#odds").val().split('|')[0]) * parseFloat($("#odds").val().split('|')[1]) * 2).toFixed(2);
        var TipStr = "";
        if (subType == "1") {
            TipStr = '您要发起比拼擂台方案，详细内容：\n\n';
            TipStr += "　　场　数：　" + $("input[type='checkbox']:checked").length + "\n";
            TipStr += "　　倍　数：　" + $("#beishu").val() + "\n";
            TipStr += "　　奖  金：　" + oddsMessage + "\n";
            TipStr += "　　玩  法：　擂台-[" + $("#playname").val() + "]\n";
            TipStr += "　　提  示：　每天仅能提交1个擂台方案 \n\n";
            TipStr += "按“确定”即表示立即提交方案，确定要提交方案吗？";
        } else {
            TipStr += "按“确定”即表示保存方案，确定要保存方案吗？";
        }

        if (!confirm(TipStr)) {
            return;
        }
        else {
            if (_buyState) {
                _buyState = false;
                $.ajax({
                    type: "post",
                    url: "Buy_Challenge_Handler.ashx",
                    data: {
                        lotid: $("#lotid").val(),
                        playid: $("#playid").val(),
                        ggtypeid: $("#ggtypeid").val(),
                        codes: $("#codes").val(),
                        zhushu: $("#zhushu").val(),
                        beishu: $("#beishu").val(),
                        odds: $("#odds").val(),
                        type: subType
                    },
                    dataType: "text",
                    success: backFunction,
                    error: function(a, b, c) {
                        _buyState = true;
                    }
                });
            }
        }
    }
}


backFunction = function(responseResult) {
    _buyState = true;
    if (responseResult == "true") {
        msg("您的方案已经提交成功，谢谢！", true);
        return;
    } else {
        if (responseResult == "ok") {
            msg("您的方案已经保存成功，谢谢！", true);
            return;
        }
        if (responseResult) {
            msg(responseResult, true);
            return;
        } else {
            msg("[竞彩擂台]对不起，每天仅能提交1个竞猜方案", true);
            return;
        }
    }
}


// 附加

$("#op_col").change(function() {
    switch (this.value) {
        case '99家平均': updatePn.init('../xml/Average99.xml?t=' + (new Date()).getTime().toString(), 2000); return;
        case '威廉希尔': updatePn.init('../xml/Willhill.xml?t=' + (new Date()).getTime().toString(), 2000); return;
        case '立博': updatePn.init('../xml/Lad.xml?t=' + (new Date()).getTime().toString(), 2000); return;
        case 'Bet365': updatePn.init('../xml/Bet365.xml?t=' + (new Date()).getTime().toString(), 2000); return;
        case '澳门彩票': updatePn.init('../xml/Macau.xml?t=' + (new Date()).getTime().toString(), 2000); return;
        default: return;
    }
});

updatePn = {
    init: function(url, time) {
        var NS = this;
        http(url, function(h, x) {
            _each($('m', x), function(i) {
                NS.parse(this)
            })
        })
    },
    parse: function(line) {
        var map = document.getElementById('vs' + _attr(line, 'id'));
        if (map) {
            var New = [];
            New[0] = _attr(line, 'win');
            New[1] = _attr(line, 'draw');
            New[2] = _attr(line, 'lost');
            attrinnerText(map, 'odds', New)
        }
    },
    onChange: function(vsTr, xmlNode) { }
}


function http(url, opts) {
    var chk = 0, o = __XHR(), no = Function(),
	        ini = {
	            type: 'GET',
	            search: null,
	            load: no,
	            err: no,
	            updated: true
	        };
    if (typeof opts == 'function') opts = { load: opts }; //for(url,fn)
    for (var key in opts) ini[key] = opts[key];
    o.open(ini.type, url, true);
    if (ini.updated) chk = http[url] || 0; //get prev modifiedTime
    if (ini.type == 'POST') o.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
    o.setRequestHeader('If-Modified-Since', chk);
    o.onreadystatechange = function() {
        if (o.readyState != 4) return;
        var ok = o.status == 200,
		        old = o.status == 304;
        if (ok) http[url] = o.getResponseHeader('Last-Modified');
        old || ok ? ini.load(o.responseText, o.responseXML, o.status) : ini.err(o.status)
    };
    o.send(ini.search);
    return o
};
var __XHR = (function(ie) {
    if (!ie) return function() { return new XMLHttpRequest };
    var n = 'Microsoft.XMLHTTP';
    try {
        new ActiveXObject(n)
    } catch (e) {
        n = "Msxml2.XMLHTTP"
    }
    return function() {
        return new ActiveXObject(n)
    }
})(!!window.ActiveXObject);

function _each(arr, fn, a, b) {
    var l = arr.length, a = parseInt(a) || 0, b = parseInt(b) || l;
    if (a < 0) a = Math.max(0, l + a);
    if (b < 0) b = Math.max(0, l + b);
    for (var i = a, j = 0; i < b; ++i, ++j)
        if (false === fn.call(arr[i], j, arr)) break;
    return b - a
};
function _attr(o, key, val) {
    if (arguments.length == 3) {
        o.setAttribute(key, val);
        return o
    };
    var val = o.getAttribute(key);
    return val ? val.toString() : ''//bug: Maxthon reutrn object
};
function attrinnerText(o, key, val) {
    var i = 0;
    for (var j = 0; j < o.cells.length; j++) {
        var td = o.cells[j];
        if (td.className == key) {
            if (isIE()) {
                td.innerText = val[i];
            }
            else {
                td.textContent = val[i];
            }
            i++;
        }
    }
    return o
};
function isIE() { //ie? 
    if (window.navigator.userAgent.toLowerCase().indexOf("msie") >= 1)
        return true;
    else
        return false;
};