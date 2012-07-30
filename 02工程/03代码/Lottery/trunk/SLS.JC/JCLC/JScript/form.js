setTimeout(function() {
    //  var config = /^.+project_fq_(.+?)(d|g)g\.aspx.*$/ig.exec(location.href);
    //  if(config == null)return;
    var playid = $id("playid").value;
    var lotType = ({						//mr=mathResult
        "7301": ["sf"],
        "7303": ["sfc"],
        "7302": ["rfsf"],
        "7304": ["dxf"]
    })[playid], C = 2;
    function getV(v) {
        if (lotType == 'bf') return selectTable.__valToString[v];
        if (lotType == 'bqc') return selectTable.__valToCodes[v];
        return +v;
    }

    //ajax提交
    function submitByAjax(form) {
        var json = {
            type: "ajax",
            url: form.getAttribute("ajax"),
            form: form,
            onsuccess: function() {
                alert("提交成功！");
            },
            onfail: function() {
                msg("提交失败！");
            }
        };

        request(json);
    }

    //AJAX类,默认post异步
    request = function(json) {
        var isPost = json.method != "get";
        var arg = json.arg || {};
        json.form && merge(arg, getForm(json.form));
        if (isPost) {
            arg = add("", arg).slice(1);
        } else {
            json.url = add(json.url, arg);
        }
        var xmlHTTP = xmlHttp();
        xmlHTTP.open(json.method || "post", json.url, json.async || true);
        if (isPost) {
            xmlHTTP.setRequestHeader("Content-Length", arg.length);
            xmlHTTP.setRequestHeader("Content-Type", "application/x-www-form-urlencoded" + (json.charset ? ";charset=" + json.charset : ""));
        }
        xmlHTTP.onreadystatechange = onStateChange(xmlHTTP, json.onsuccess, json.onfail);
        xmlHTTP.send(isPost ? arg : null);
        tb_remove();
    }

    //状态改变时
    onStateChange = function(xmlHTTP, success, fail) {
        return function() {
            if (xmlHTTP.readyState != 4) return;
            var o = result(xmlHTTP);
            if (o.success && success) {
                alert("您的方案已经提交成功，谢谢！");
                location.reload();
            }
            else if (!o.success && fail) {
                tb_remove();
                $id('dgBtn').disabled = false;

                if (o.text.indexOf("BuyID") >= 0) {
                    if (!confirm("您的余额不足，点击确定将转跳到充值页面进行充值！")) {
                        return false;
                    }

                    window.document.location.href = "../Home/Room/OnlinePay/Default.aspx?" + o.text;
                    return;
                }

                msg(o.text);
            }
        }
    }

    //返回结果时
    result = function(xmlHTTP) {
        if ((xmlHTTP.status == 0 || xmlHTTP.status == 200) && xmlHTTP.responseText == "true") {
            var txt = xmlHTTP.responseBody;
            return { success: true, text: txt };
        }

        return { success: false, text: xmlHTTP.responseText };
    }

    merge = function(o1, o2) {
        for (var i in o2) {
            o1[i] = o2[i];
        }
        return o1;
    }

    getForm = function(form) {
        form = typeof (form) == "string" ? document.forms[form] : form;
        var o = {}, t;
        for (var i = 0, l = form.elements.length; i < l; i++) {
            t = form.elements[i];
            t.name != "" && (o[t.name] = t.type == "radio" ? getRadio(t.name) : t.value);
        }
        return o;
    }

    getRadio = function(name) {
        var o = getName(name);
        for (var i = o.length - 1; i > -1 && !o[i].checked; i--);
        return i > -1 ? o[i].value : false;
    }

    getName = function(name) {
        return document.getElementsByName(name);
    }

    add = function(u, o) {
        var a = [];
        for (var i in o) {
            if (i == "lotid" || i == "playid" || i == "ggtypeid" || i == "codes" || i == "zhushu" || i == "beishu" || i == "totalmoney" || i == "tb_SchemeBonusScale" || i == "tb_Share" || i == "AssureMoney" || i == "tb_BuyShare" || i == "SecrecyL" || i == "tb_Title") {
                a.push(i + "=" + o[i]);
            }
        }
        return u + "?" + a.join("&");
    }

    xmlHttp = function() {
        var xmlHttpRequest = null; //这里是大家都常用的IE，firefox中取得XMLHttpRequest对象的方法
        try {

            xmlHttpRequest = new XMLHttpRequest();
        }
        catch (e) {
            try {
                xmlHttpRequest = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");

            }
        }

        return xmlHttpRequest;
    }

    function getCodes(l) {
        var res = [];
        each(l, function() {
            if (this.style.display != 'none') res.push(getV(this.firstChild.value));
        });
        return '(' + res.join(',') + ')';
    }

    if (!$("#btn_BuyConfirm")) return;
    $("#btn_BuyConfirm").click(function() {
        var bs = +$id('beishu').value, zs = +$id('zhushu').value;
        var res = [];
        var Dan = [];
        each(selectTable.box.rows, function(i) {
            if (this.style.display != 'none') {
                if (this.cells.length > 3 && this.cells[3].getElementsByTagName('input')[0].checked) {
                    Dan.push({
                        preFix: this.preFix,
                        date: +new Date(this.date.replace(/-/g, '/')),
                        pname: parseInt(this.pname),
                        code: getCodes(this.cells[2].getElementsByTagName('label'))
                    })
                }
                else {
                    res.push({
                        preFix: this.preFix,
                        date: +new Date(this.date.replace(/-/g, '/')),
                        pname: parseInt(this.pname),
                        code: getCodes(this.cells[2].getElementsByTagName('label'))
                    })
                }
            }
        });

        res.sort(function(a, b) {
            if (a.date == b.date) {
                return a.pname - b.pname
            } else {
                return a.date - b.date
            };
        });

        Dan.sort(function(a, b) {
            if (a.date == b.date) {
                return a.pname - b.pname
            } else {
                return a.date - b.date
            };
        });

        var map = [];
        var mapDan = [];

        each(res, function() {
            map.push(this.preFix + this.code)
        });
        each(Dan, function() {
            mapDan.push(this.preFix + this.code)
        });

        var curType = "";
        each($id('input', 'ggList'), function() { if (this.checked) { curType += ggAdmin.ggValue[this.value] + $id('beishu').value + ","; } })
        curType = curType == "" ? curType : curType.substring(0, curType.length - 1);

        if (!$id('ggList')) {
            curType = "A0" + $id('beishu').value;
        }

        if (mapDan.length > 0) {
            $id('codes').value = $id('playid').value + ';[' + mapDan.join('|') + '][' + map.join('|') + '];[' + curType + "];[" + $id('dpUnsureDan').value + "]";
        }
        else {
            $id('codes').value = $id('playid').value + ';[' + map.join('|') + '];[' + curType + "]";
        }

        var form = document.buy_form;
        submitByAjax(form);
    });

    if (!$id('dgBtn')) return;
    $id('dgBtn').onclick = function() {
        if (!CreateLogin(this)) {
            return;
        }

        var bs = +$id('beishu').value, zs = +$id('zhushu').value;
        if (selectTable.box.rows.length < 1) return msg('请至少选择' + C + '场以上的比赛!');
        if (isNaN(zs) || zs < 1) return msg('请选择过关方式');
        if (isNaN(bs) || bs < 1) return msg('倍数填写不正确!');
        if (parseInt($id('totalmoney').value, 10) > 2000000) return msg('方案总金额不能超过2000000!\n请修改投注内容后重试.');

        $("#span_playtype").html($id("playname").value);
        $("#span_zs").html(zs);
        $("#span_bs").html(bs);
        $("#span_totalmoney").html($id('totalmoney').value);
        $("#span_share").html($id('tb_Share').value);
        $("#span_sharemoney").html($id('lab_ShareMoney').innerText);
        $("#span_assuremoney").html($id('lab_AssureMoney').innerText);
        $("#span_buymoney").html($id('lab_BuyMoney').innerText);
        $("#AssureMoney").val($id('lab_AssureMoney').innerText)

        tb_show("确认方案投注内容", "#TB_inline?height=300&amp;width=380&amp;inlineId=tab_box", "");
    }

    if (!$id('FilterBtn')) return;
    $id('FilterBtn').onclick = function() {
        var curType = "";
        var Msg = "";
        each($id('input', 'ggList'), function() {
            if (this.checked) {
                if (curType != "") {
                    Msg = "高级过滤不支持多选组合过关，请只选择一个过关方式!";
                    return false;
                }

                if (this.value.substring(this.value.length - 2) != "串1") {
                    Msg = "高级过滤目前只支持 N 串 1 的过关方式!";
                    return false;
                }
                curType += ggAdmin.ggValue[this.value] + $id('beishu').value;
            }
        })

        if (Msg != "") {
            msg(Msg);
            return;
        }

        var res = [];
        each(selectTable.box.rows, function(i) {
            if (this.style.display != 'none') {
                res.push({
                    preFix: this.preFix,
                    date: +new Date(this.date.replace(/-/g, '/')),
                    pname: parseInt(this.pname),
                    code: getCodes(this.cells[2].getElementsByTagName('label'))
                })
            }
        });

        res.sort(function(a, b) {
            if (a.date == b.date) {
                return a.pname - b.pname
            } else {
                return a.date - b.date
            };
        });

        var map = [];
        each(res, function() {
            map.push(this.preFix + this.code)
        });

        var Code = $id('playid').value + ';[' + map.join('|') + '];[' + curType + "]";

        openPostWindow('../JCZC/FilterShrink.aspx', { Number: Code }, "_blank")//过滤
    }

    var c = cookie('jczq_info');
    if (c == null || c == '') return;
    var info = false;
    try {
        info = eval('(' + c + ')');
    } catch (e) { return; }
    $id('buybs').value = $id('beishu').value = info.beishu;
    $id('ggtypeid').value = info.sgtype;
    cookie('jczq_info', '', { time: -10 });
    c = info.codes.split('/');
    var r = /^(\d+?)\|\d+\[(.+?)\]$id/i, s, m, n, j, k, map = {
        'sf': { '1': 1, '2': 0 }, //胜负，主负在前、主胜在后
        'rfsf': { '1': 1, '2': 0 }, //让分胜负，主负在前、主胜在后
        'sfc': [
      { '11': 0, '12': 1, '13': 2, '14': 3, '15': 4, '16': 5 },
      { '01': 0, '02': 1, '03': 2, '04': 3, '05': 4, '06': 5 }
    ], //胜分差
        'dxf': { '1': 0, '2': 1}//大小分
}[lotType];

        for (var i = c.length; i--; ) {//重现选择的场次
            s = r.exec(c[i]);
            if (s == null) continue;
            m = $id('vs' + s[1]) || $id('pltr_' + s[1]);
            if (m == null) continue;
            if (m.parentNode.style.display == 'none') opendate(m.parentNode.id.split('_')[1]);
            if (lotType == 'bf') openclose(s[1]);
            n = s[2].split(',');
            each(n, function(a) {
                if (lotType != 'sfc') {
                    m.options[map[n[a]]].click();
                } else {
                    k = 0;
                    if (map[1][n[a]] != undefined) k = 1;
                    m.options[k].options[map[k][n[a]]].click();
                }
            });
        }
        if ($id('selectMatchNum') != null) return;
        setTimeout(function() {
            for (var t in ggAdmin.ggValue) {//重现选择的玩法
                if (ggAdmin.ggValue[t] == info.sgtype) {
                    $id('r' + t.replace(/串/g, 'c')).click();
                    break;
                }
            }
        }, 20);
    }, 50);
