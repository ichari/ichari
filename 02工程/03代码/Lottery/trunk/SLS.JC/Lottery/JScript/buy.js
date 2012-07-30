var cd = CountDown;
var Duplex = false;
var bScalec = 0;
var LScaleMoney = 100;
var LScale = 0.2;
var UScaleMoney = 10000;
var UpperScale = 0.05;

$().ready(function() {
    $("#buyTab li").each(function(i) {//check事件
        this.onclick = function() {
            $(this).attr("class", "active").siblings().attr("class", "normal");
            if ($(this).attr("mid") == "1") {
                location.href = "/Join/Project_List.aspx?id=" + Config.lot;
                return;
            }
        }
    });

    $(".caip_numb a").each(function(i) {//check事件
        this.onclick = function() {
            $(this).attr("class", "on").siblings().removeClass("on");
            $("#HidIsuseName").val($(this).attr("name"));
            $("#HidIsuseID").val($(this).attr("mid"));
            if (Config.IsgetXml)
            { getXML(); }
            else {
                cd.IsuseEndTime = $(".endtime .c_999").text();
                cd.serverTime = $('#HidServerTime').val();
                cd.timebar = $('#countDownSpan');
                cd.startclock();
            }
        }
    });

    $.ajax({
        url: '/ajax/getBonusScalec.ashx',
        type: 'GET',
        dataType: 'json',
        timeout: 20000,
        error: function(json) {return;},
        success: function(json) {
            bScalec = parseFloat(json.bScalec);
            LScaleMoney = parseInt(json.LScaleMoney);
            LScale = parseFloat(json.LScale);
            UScaleMoney = parseInt(json.UScaleMoney);
            UpperScale = parseFloat(json.UpperScale);
            $("#SchemeBonusScale").val(bScalec);
        }
    });

    $("#pt_put").click(function() {
        if (table_vs.SumCount < 1) {msg("您好，请您最少选择一注投注号码！");}
        else if (Duplex && table_vs.SumCount < 2) { msg("您好，您选择的不是一注复式号码！");}
        else {addTradeList();CalcResult();table_vs._clearAll_();}
    });

    $("#btn_ClearSelect").click(function() {
        if ($("#list_LotteryNumber option:selected").length > 0) {
            $("#list_LotteryNumber option:selected").each(function() {
                $("#fb_list").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option");
                $("#lab_Num").text(parseInt($("#lab_Num").text()) - parseInt($(this).val()));
                $("#lab_SumMoney").text(parseInt($("#lab_SumMoney").text()) - parseInt($(this).val()) * 2 * parseInt($("#input_Multiple").val()));
                $(this).remove();
            })
        }
        else {msg("您还没有输入投注内容！");}

        $("#list_LotteryNumber option").each(function() {
            $("#HidLotteryNumber").val($("#HidLotteryNumber").val() + $(this).text() + "\n");
        })

        return true;
    });

    $("#btn_UploadScheme").click(function() {
        btn_UploadScheme();
    });

    $("#btnPaste").click(function() {
        $("#tbLotteryNumbers").val('');
        $("#frame_Upload").attr("src", "/Lottery/SchemeUpload.aspx?id=" + Config.lot + "&PlayType=" + Config.PlayTypeID + "&Isuse=" + $("#HidIsuseID").val());
        tb_show("方案粘贴上传", "#TB_inline?height=270&amp;width=516&amp;inlineId=div_Uplaod", "");
    });

    $("#btn_Clear").click(function() {
        ClearResult();
        return true;
    });

    $("#Share, #BuyShare, #AssureShare").blur(function() {
        CalcResult();
        return;
    });

    //预投
    $("#SchemeMoney").blur(function() {
        $("#smErr").hide();
        $("#smsErr").hide();

        var SumMoney = parseFloat($("#SchemeMoney").val());

        if ((SumMoney % 2 != 0) || SumMoney < 2) {
            $("#smErr").show();
            return;
        }

        if (Duplex && SumMoney % 4 != 0) {
            $("#smsErr").show();
            return;
        }

        $("#lab_SumMoney").text($("#SchemeMoney").val());
        $("#lab_Num").text(SumMoney / 2);

        CalcResult();
        return;
    });

    $("#input_Multiple").bind({
        change: function() {
            $("#lab_SumMoney").text(parseInt($("#lab_Num").text()) * parseInt($("#input_Multiple").val()) * 2);
            return true;
        },
        blur: function() {
            $("#lab_SumMoney").text(parseInt($("#lab_Num").text()) * parseInt($("#input_Multiple").val()) * 2);
            CalcResult();
            return true;
        }
    });

    $("#SchemeBonusScale").bind({
        change: function() {
            if (parseInt($("#SchemeBonusScale").val()) > 10) {
                $("#SchemeBonusScale").val(10);
            }
            return true;
        },
        blur: function() {
            if (parseInt($("#SchemeBonusScale").val()) > 10) {
                $("#SchemeBonusScale").val(10);
            }
            return true;
        }
    });

    $("#divControl").click(function() {
        if (this.checked) {
            $("#myhemai").attr("display", "block");
            if (parseFloat($("#lab_SumMoney").text()) > 0) {
                $("#Share").val($("#lab_SumMoney").text());
            }
            CalcResult();
        }
        else {
            $("#myhemai").attr("display", "none");
        }
    });

    $("#input_Multiple").numeral();
    $("#SchemeBonusScale").numeral();
    $("#Share").numeral();
    $("#BuyShare").numeral();
    $("#AssureShare").numeral();
    $("#SchemeMoney").numeral();

    $("#dgBtn").click(function() {
        if (!CreateLogin(this)) {
            return;
        }

        if (!$("#chkAgrrement").attr("checked")) {
            msg("请先阅读用户电话短信投注协议，谢谢！");
            return false;
        }

        if ($("#countDownSpan").text() == "已截止" < 0) {
            msg("本期投注已截止，谢谢。");
            return false;
        }

        if ($("#HidLotteryNumber").val() == "" && Config.IsShow) {
            msg("请选择你需要购买的内容。");
            return false;
        }

        var Mulitple = 0;
        Mulitple = parseInt($("#input_Multiple").val());

        if (Mulitple < 1 || Mulitple > 999) {
            msg("请输入正确的倍数！");
            $("#input_Multiple").focus();
            return false;
        }

        var _Share = $("#Share");
        var SumMoney = parseFloat($("#lab_SumMoney").text());
        var Share = parseFloat(_Share.val());
        var ShareMoney = (SumMoney / Share).toFixed(2);
        var ShareMoney2 = Math.round(SumMoney / Share * 1) / 1;

        var _BuyShare = $("#BuyShare");
        $("#ShareMoney").text(ShareMoney);
        $("#BuyMoney").text(ShareMoney * parseFloat(_BuyShare.val()));

        if (ShareMoney != ShareMoney2 || ShareMoney < 1) {
            $("#fsErr").show();
            _Share.focus();
            return;
        }

        $("#BuyMoney").text(ShareMoney * parseFloat(_BuyShare.val()));
        if (parseInt(_BuyShare.val()) > Share) {
            $("#buyfsErr").show();
            _BuyShare.focus();
            return;
        }

        var _AssureShare = $("#AssureShare");
        $("#AssureMoney").text(ShareMoney * parseFloat(_AssureShare.val()));
        if (parseInt(_BuyShare.val()) + parseInt(_AssureShare.val()) > Share) {
            $("#AssurefsErr").show();
            _AssureShare.focus();
            return;
        }

        if ((LScaleMoney > 0) && (UScaleMoney > LScaleMoney) && (UpperScale > 0) && (LScale > UpperScale)) {
            if (SumMoney <= LScaleMoney) {
                bScalec = LScale;
            }
            else if (SumMoney >= UScaleMoney) {
                bScalec = UpperScale;
            }
            else {
                bScalec = LScale - ((SumMoney - LScaleMoney) * ((LScale - UpperScale) / (UScaleMoney - LScaleMoney)));
            }
        }
        else if (LScale <= UpperScale) {
            bScalec = LScale;
        }

        if (bScalec <= 0) {
            bScalec = 0.1;
        }

        if (($("#BuyShare").val()) < Math.round(Share * bScalec)) {
            if (LScale == UpperScale) {
                msg("发起人最少必须认购 " + (LScale * 100) + "%。(" + Math.round(Share * LScale) + ' 份， ' + (Math.round(Share * LScale) * StrToFloat(o_lab_ShareMoney.innerText)) + ' 元)');
            }
            else {
                msg("此方案发起人认购(不含保底)最少必须达到 " + Math.round(Share * bScalec) + " 份。\r\n\r\n" +
                    "发起方案最少认购比例说明：\r\n" +
                    "方案金额小于或等于 " + LScaleMoney + " 元，最少认购 " + LScale * 100 + "%，\r\n" +
                    "方案金额大于或等于 " + UScaleMoney + " 元，最少认购 " + UpperScale * 100 + "%，\r\n" +
                    "方案金额在 " + LScaleMoney + " 元至 " + UScaleMoney + " 元之间的最少认购比例平滑递减。\r\n\r\n" +
                    "此方案金额的最少认购比例是 " + Round(bScalec, 2) * 100 + "% 。");
            }

            $("#BuyShare").focus();
            return false;
        }

        $('dgBtn').attr("disabled", "disabled");

        var Datajson = "lotid=" + Config.lot + "&playid=" + Config.PlayTypeID + "&codes=" + $("#HidLotteryNumber").val() + "&zhushu=" + $("#lab_Num").text() + "&beishu=" + $("#input_Multiple").val() +
            "&totalmoney=" + $("#lab_SumMoney").text() + "&bScalec=" + $("#SchemeBonusScale").val() + "&Share=" + Share + "&AssureMoney=" + $("#AssureMoney").text() + "&BuyShare=" + $("#BuyShare").val() + "&SecrecyL=" +
            $("input[name='SecrecyLevel'][type='radio']:checked").val() + "&Title=" + $("#Title").val() + "&IsuseID=" + $("#HidIsuseID").val() + "&EndTime=" + $(".endtime .c_999").text()

        $.ajax({
            url: '/ajax/SubmitScheme.ashx',
            type: 'GET',
            dataType: 'json',
            cache: false,
            async: false,
            data: Datajson,
            timeout: 20000,
            beforeSend: function() { $("#divload").show(); }, //发送数据之前
            error: function(json) {
                msg(json.message);
                return;
            },
            success: function(json) {
                if (isNaN(json.message)) {
                    msg(json.message);

                    return;
                }

                location.href = "/Home/Room/UserBuySuccess.aspx?LotteryID=" + Config.lot + "&Money=" + $("#lab_SumMoney").text() + "&SchemeID=" + json.message;
                return;
            }
        });
    });
});

ThreeYuan = function (A, B) {
    if (A == null || A == "") {A = B;}
    return A;
}

var table_vs = {
    LotteryNumber: "",
    SumCount: 1,

    init: function(tableID) {
        this.table = _find(tableID);
        var list = this.table.rows;
        this.list = [];
        for (var i = list.length; i--; ) this.__setRow__(list[i]);
        _find("BetInfo").innerHTML = "0 注，0.00 元";
    },

    __getSpans__: function(tr) {
        var span = _find('span', tr);
        if (span == null) return;
        var len = span.length;
        if (len < 1) return;
        var body = [];
        each(span, function(i) {
            body.push(this)
        }, 0);
        return {
            head: span[0],
            options: body
        }
    },

    __setRow__: function(tr) {
        var span = this.__getSpans__(tr);
        if (!span) return;

        var NS = this;
        this.list.push(tr);

        tr.onmouseover = function() { $(this).addClass("th_on"); }
        tr.onmouseout = function() { $(this).removeClass("th_on"); }

        if (span.options.length == 0) return;
        tr.options = span.options;
        var opts = span.options;

        Y = this;

        for (var i = opts.length - 1; i >= 0; i--) {
            opts[i].className = "ball_num";
            opts[i].onclick = function(e) {
                if (this.className != "ball_num") { this.className = "ball_num"; }
                else {
                    if (!Duplex) { Y._clearRow_(opts); }
                    this.className = "ball_num_red";
                }
                Y.__getCount_(this);
            };
        };
    },

    _clearAll_: function() {
        var list = this.table.rows;
        for (var i = list.length - 1; i >= 0; i--) {
            var span = this.__getSpans__(list[i]);
            if (span == null || span.options.length == 0) break;
            var opts = span.options;
            for (var j = opts.length - 1; j >= 0; j--) {
                opts[j].className = "ball_num";
            }
        }

        table_vs.SumCount = 0;
        table_vs.LotteryNumber = "";

        $("#pt_put").attr("src", "images/btn_caipadd_hui.gif");
        _find("BetInfo").innerHTML = "0 注，0.00 元";
    },

    _clearRow_: function(opts) {
        for (var i = opts.length - 1; i >= 0; i--) {
            opts[i].className = "ball_num";
        };
    },

    __getCount_: function() {
        var list = this.table.rows;
        var RowCount, ListNumber;
        Y = this;
        Y.SumCount = 1, Y.LotteryNumber = "";
        for (var i = 1; i < list.length; i++) {
            RowCount = 0, ListNumber = "";
            var span = this.__getSpans__(list[i]);

            if (span == null || span.options.length == 0) break;
            var opts = span.options;

            for (var j = opts.length - 1; j >= 0; j--) {
                if (opts[j].className != "ball_num") {
                    RowCount++;
                    ListNumber += opts[j].innerText ? opts[j].innerText : opts[j].textContent;
                }
            }

            Y.SumCount *= RowCount;

            if (RowCount == 0) break;
            if (RowCount > 1) ListNumber = "(" + ListNumber + ")";
            Y.LotteryNumber += ListNumber.replace("+", "");
        }
        Y.SumCount > 0 ? $("#pt_put").attr("src", "images/btn_caipadd.gif") : $("#pt_put").attr("src", "images/btn_caipadd_hui.gif");
        _find("BetInfo").innerHTML = Y.SumCount + " 注，" + Y.SumCount * 2 + ".00 元";
    }
}


btn_UploadScheme = function() {
    $.ajax({
        url: '/ajax/AnalyseScheme.ashx',
        type: 'GET',
        dataType: 'json',
        cache: false,
        async: false,
        data: "LotteryID=" + Config.lot + "&PlayTypeID=" + Config.PlayTypeID + "&Content=" + $("#tbLotteryNumbers").val().replace(/['\n']+/g, '|'),
        timeout: 20000,
        beforeSend: function() { $("#divload").show(); }, //发送数据之前
        error: function(json) {
            msg(json.message);
            return;
        },
        success: function(json) {
            if (isNaN(json.message)) {
                msg(json.message);

                return;
            }

            tb_remove();

            var LotteryNumber = json.Result.replace(/['#']+/g, '\n');

            if (LotteryNumber == null || LotteryNumber == "") {
                msg("从方案文件中没有提取到符合书写规则的投注内容。");

                return;
            }

            ClearResult();

            var Lotterys = LotteryNumber.split("\n");

            for (var i = 0; i < Lotterys.length; i++) {
                var str = Lotterys[i];
                if (str == "")
                    continue;
                strs = str.split("|");

                if (strs.length != 2) {
                    continue;
                }

                str = strs[0];
                if (str == "") {
                    continue;
                }
                var Num = parseInt(strs[1]);
                if (Num < 1)
                    continue;

                addTradeList(str, Num);
            }
            return;
        }
    });
}

addTradeList = function(Number, Count) {
    Count = Count || table_vs.SumCount;
    Number = Number || table_vs.LotteryNumber;

    $("#list_LotteryNumber").append("<option value='" + Count + "'>" + Number + "</option>");
    $("#HidLotteryNumber").val($("#HidLotteryNumber").val() + Number + "*");
    $("#lab_Num").text(parseInt($("#lab_Num").text()) + Count);
    $("#lab_SumMoney").text(parseInt($("#lab_SumMoney").text()) + Count * 2 * parseInt($("#input_Multiple").val()));
}

getXML = function() {
    $.ajax({
        url: '/Xml/' + Config.lotCode + '_' + $("#HidIsuseName").val() + '.xml?rnd=' + Math.random(),
        type: 'GET',
        dataType: 'xml',
        timeout: 20000,
        error: function(xml) {
            $(".buy_way li").slice(0, 1).hide();
            $(".buy_way li").slice(1, 2).hide();
            $(".buy_way li").slice(2, 3).click();
            return;
        },
        success: function(xml) {
            $(xml).find("row").each(function(i) {
                Redraw(this, i);
            });
            $(xml).find("head").each(function(i) {
                $(".endtime .c_999").text($(this).attr("EndTime"));
                var htmlobj = $.ajax({ url: "/ajax/getServerTime.ashx?rnd=" + Math.random(), async: false });
                $('#HidServerTime').val(htmlobj.responseText);
                cd.IsuseEndTime = $(".endtime .c_999").text();
                cd.serverTime = $('#HidServerTime').val();
                cd.timebar = $('#countDownSpan');
                cd.startclock();
            });
        }
    });
}

CalcResult = function() {
    $("#fsErr").hide();
    $("#buyfsErr").hide();
    $("#AssurefsErr").hide();

    var SumMoney = Config.IsShow ? parseFloat($("#lab_SumMoney").text()) : parseFloat($("#SchemeMoney").val());
    var Share = parseFloat($("#Share").val());
    if (!Share) { Share = 1; $("#Share").val(SumMoney); }

    var ShareMoney = (SumMoney / Share).toFixed(2);
    var ShareMoney2 = Math.round(ShareMoney * 1) / 1;

    $("#ShareMoney").text(ShareMoney);
    $("#BuyMoney").text(ShareMoney * parseFloat($("#BuyShare").val()));

    if (ShareMoney != ShareMoney2) {
        $("#fsErr").show();
    }

    var BuyShare = $("#BuyShare").val();
    if (!BuyShare) { BuyShare = 1; $("#BuyShare").val(1); }

    $("#BuyMoney").text((ShareMoney * BuyShare).toFixed(2));
    if (parseInt($("#BuyShare").val()) > Share) {
        $("#buyfsErr").show();
        return;
    }

    var AssureShare = $("#AssureShare").val();
    if (!AssureShare) { AssureShare = 0; $("#AssureShare").val(0); }

    $("#AssureMoney").text((ShareMoney * parseFloat($("#AssureShare").val())).toFixed(2));
    if (parseInt($("#BuyShare").val()) + parseInt($("#AssureShare").val()) > Share) {
        $("#AssurefsErr").show();
    }
}

ClearResult = function() {
    table_vs._clearAll_();
    $("#lab_Num").text(0);
    $("#lab_SumMoney").text(0);
    $("#HidLotteryNumber").val("");
    $("#input_Multiple").val("1");
    $("#Share").val("1");
    $("#BuyShare").val("1");
    $("#AssureShare").val("0");
    CalcResult();

    $("#list_LotteryNumber option").each(function() {
        $(this).remove();
    })
}

_find = function(id, owner) {
    if (owner !== undefined) {
        owner = _find(owner);
        return owner == null ? [] : owner.getElementsByTagName(id)
    };
    return typeof id == 'string' ? document.getElementById(id) : id
};

each = function(arr, fn, a, b) {
    var l = arr.length, a = parseInt(a) || 0, b = parseInt(b) || l;
    if (a < 0) a = Math.max(0, l + a);
    if (b < 0) b = Math.max(0, l + b);
    for (var i = a, j = 0; i < b; ++i, ++j)
        if (false === fn.call(arr[i], j, arr)) break;
    return b - a
};