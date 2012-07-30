var $id = find, selectedColor = "td_click";
/*
VStable 对阵选择器
-------------------------------------------------------------------------------------------*/
var AutoClick = function() {
    if ((this.childNodes[0].tagName != undefined) && (this.childNodes[0].tagName.toLowerCase() == 'input')) return this.childNodes[0].click();
    this.childNodes[1].click();
};
var table_vs = {

    __hideCount: 0,
    PnShowTime: -1,

    init: function(tableID) {
        this.table = $id(tableID);
        var list = this.table.rows;
        this.list = [];
        for (var i = list.length; i--; ) this.__setRow__(list[i]); //迭代设置check
    },

    __getChks__: function(tr) {
        var chks = $id('INPUT', tr);
        var len = chks.length;
        if (len < 1) return false;
        var body = [];
        each(chks, function(i) {
            body.push(this)
        }, 1, -1);
        return {
            head: chks[0],
            options: body,
            allSelect: chks[len - 1]
        }
    },

    __updatePn__: function(tr, data) {
        var d = 0, t = 0, old = { win: attr(tr, 'win'), draw: attr(tr, 'draw'), lost: attr(tr, 'lost') };
        var view = {
            win: tr.options[0].nextSibling,
            draw: tr.options[1].nextSibling,
            lost: tr.options[2].nextSibling
        };

        for (var k in old) {
            d = data[k] - old[k];
            if (isNaN(parseInt(data[k]))) return;
            if (d != 0) {
                t = parseFloat(data[k]);
                view[k].innerHTML = t == 0 ? '' : (data[k] + (d > 0 ? '<span style="color:#F00;font-weight:normal;">↑</span>' : '<span style="color:#090;font-weight:normal;">↓<span>'));
                view[k]._data = t == 0 ? '' : data[k];
                view[k].title = (d > 0 ? '+' : '') + d.toFixed(2);
                attr(tr, k, data[k]);

                var Hz = this.PnShowTime;
                if (Hz != -1) {//上下箭头延时消失
                    (function(el, html) {
                        setTimeout(function() {
                            el.innerHTML = html;
                        }, Hz);
                    })(view[k], t == 0 ? '' : data[k])
                }

            }
        };
    },
    __setRow__: function(tr) {
        var chks = this.__getChks__(tr);
        if (!chks) return;

        var NS = this;
        this.list.push(tr);
        tr.hide = function() {//行隐藏
            this.style.display = 'none';
            NS.onHide(this, this.__hideCount++)
        };
        tr.show = function() {//行显示
            tr.style.display = '';
        };
        chks.head.onclick = function() {//隐藏[sender:chk,influence:tr]
            this.checked = true;
            tr.hide()
        };

        tr.onmouseover = function() {
            this.style.backgroundColor = "#87D1ED";
        }

        tr.onmouseout = function() {
            this.style.backgroundColor = "";
        }

        if (chks.options.length == 0) return;

        tr.options = chks.options;

        tr.zid = attr(tr, 'zid');
        tr.id = 'vs' + tr.zid;
        tr.rq = parseInt(attr(tr, 'rq'));

        tr.updatePn = function(data) {//更新折率
            NS.__updatePn__(this, data)
        };

        var opts = chks.options;
        for (var i = opts.length; i--; ) {//单选
            opts[i].onclick = function(e) {
                chks.allSelect.checked = NS.isAllSelect(opts);
                NS.onSelect(tr, this);
                e = e || window.event;
                if (e.preventDefault) e.stopPropagation();
            };
            opts[i].cancel = function() {//手动取消选择
                this.checked = false;
                chks.allSelect.checked = NS.isAllSelect(opts)
            }
            opts[i].parentNode.onclick = AutoClick;
            this.checked = false;
        };
        chks.allSelect.parentNode.onclick = AutoClick;
        chks.allSelect.onclick = function(e) {//全选
            for (var i = opts.length; i--; ) {
                if (opts[i].checked != this.checked) {
                    opts[i].checked = this.checked;
                    NS.onSelect(tr, opts[i])
                }
            }
            e = e || window.event;
            if (e.preventDefault) e.stopPropagation(); //Firefox下事件冒泡
        }

    },

    __valToChk: [3, 2, 0, 1],

    setCheckBox: function(zid, spf) {//外部操作checkbox
        var tr = document.getElementById('vs' + zid);
        if (tr) {
            var chks = tr.getElementsByTagName('INPUT');
            var i = this.__valToChk[spf];
            chks[i].cancel()
            chks[i].parentNode.className = 'sp_bg';
        }
    },

    showAll: function() {//显示所有
        var list = this.list, el;
        for (var i = list.length; i--; ) {
            el = list[i];
            if (el.className.indexOf('end') != -1) {
                el.style.display = 'none';
                el.className = el.className.replace('end', '')//有的地方使用了end样式控制
            };
            if (el.style.display == 'none')
                el.style.display = ''
        }
        this.onHide()
    },

    showRQ: function(type, isShow) {//显示让球
        var list = this.list;
        var check = function(o) { return o.rq != 0 };
        if (type == 'nrq') { check = function(o) { return o.rq == 0 } };
        for (var i = list.length; i--; )
            if (check(list[i]))
            isShow ? list[i].show() : list[i].hide();
    },

    showByName: function(tName, isShow) {//显隐球队
        var list = this.list;
        var check = function(o) { return attr(o, 'lg') == tName };
        for (var i = list.length; i--; )
            if (check(list[i]))
            isShow ? list[i].show() : list[i].hide();
    },

    getHideCount: function() {//输出显示总数
        var list = this.list, count = 0;
        var check = function(o) { return o.style.display == 'none' };
        for (var i = list.length; i--; )
            if (check(list[i])) count++;
        this.__hideCount = count;
        return count
    },

    isAllSelect: function(chks) {//是否选项已全选
        for (var i = chks.length; i--; ) if (!chks[i].checked) return false;
        return true
    },

    onSelect: function(tr, chk) { },
    onHide: function(tr) { }
}
/*
SelTable
------------------------------------------------------------------------------------------*/
var selectTable = {
    __namePos: [3, 5], //名称位置
    __valToString: ['负', '平', '', '胜'],

    init: function(box, tpl) {
        this.box = $id(box);
        this.tpl = $id('tr', tpl)[0];
        this.result = {
            count: $id('selCount'),
            cost: $id('outMoney'),
            back: $id('backMoney')
        }
    },

    del: function(tr, chk, map) {
        var mapTr = map || this.mapTr(tr, chk);
        var ns = this;
        if (mapTr) {
            mapTr.hide();
            //mapTr.edit.value=1;
            this.setMoney(mapTr);
        }
    },

    add: function(tr, chk) {
        if (this.mapTr(tr, chk)) return this.show(tr, chk);
        var newTr = this.getNewTr(tr, chk);
        var ns = this;
        this.box.appendChild(newTr);
        clearTimeout(this.batAddTimer);
        this.batAddTimer = setTimeout(function() {
            ns.reSort();
            ns.showMoneyInfo()
        }, 48);
    },

    __getPn__: function(tr, chk) {
        return parseFloat(attr(tr, ["lost", "draw", , "win"][chk.value])) || 2.00; //赔率
    },

    getNewTr: function(tr, chk) {
        var tmp = this.tpl.cloneNode(true), NS = this;
        var tds = tmp.getElementsByTagName('TD');
        tmp.id = 'sel' + tr.zid + '_' + chk.value;
        tmp.zid = attr(tr, 'zid'); //zid
        tmp.spf = chk.value; //310
        tmp.pn = this.__getPn__(tr, chk);
        tds[0].lastChild.innerHTML = this.unHTML(tr.cells[0].innerHTML);
        tds[0].firstChild.onclick = function() { this.checked = true; NS.hide(tmp) };
        tds[1].innerHTML = this.unHTML(tr.cells[this.__namePos[0]].innerHTML + ' vs ' + tr.cells[this.__namePos[1]].innerHTML);
        tds[2].innerHTML = (this.__valToString[tmp.spf]) + (tmp.pn - 2 > 0 ? '(' + tmp.pn.toFixed(2) + ')' : '');
        tmp.hide = function() { this.style.display = 'none' };
        tmp.show = function() { this.style.display = '' };
        /*
        tmp.edit=tds[3].firstChild;
        tmp.cost=tds[4];
        tmp.back=tds[5];
        tmp.back.innerHTML='￥'+tmp.pn.toFixed(2);
        intInputChange(tmp.edit,function (){
        NS.setMoney(tmp)
        });
        */
        return tmp
    },

    __order__: 1,

    __sort__: function() {
        var ord = this.__order__;
        return function(a, b) {
            var d1 = +new Date(a.date.replace(/-/g, '/'));
            var d2 = +new Date(b.date.replace(/-/g, '/'));
            if (d1 == d2) {
                return a.pname - b.pname
            } else {
                return d1 - d2
            };
        };
        /*return function (a,b){
        return a.zid!=b.zid?
        (a.zid-b.zid):
        ord*(b.spf-a.spf)
        }*/
    },

    reSort: function() {
        var all = [], NS = this;
        each($id('tr', this.box), function(i) { all[i] = this }); //转换为数组
        all.sort(this.__sort__()); //定序
        this.box.style.display = 'none';
        each(all, function() { NS.box.appendChild(this) })//重插入
        this.box.style.display = '';
    },

    show: function(tr, chk) {//显示
        var mapTr = this.mapTr(tr, chk);
        var ns = this;
        mapTr.show();
        clearTimeout(ns.batAddTimer);
        ns.batAddTimer = setTimeout(function() {
            ns.reSort();
            ns.showMoneyInfo()
        }, 48);
    },

    mapTr: function(tr, chk) {
        return document.getElementById('sel' + tr.zid + '_' + chk.value);
    },

    hide: function(tr) {//隐藏
        this.del(null, null, tr);
        this.onHide(tr)
    },

    unHTML: function(html) {//去html
        return html.replace(/<\/?[^>]+>/g, '')
    },

    setMoney: function(tr) {//响应倍数改变
        if (tr) {
            /*
            var tpl=['￥','0.00','元'];
            var m=1,pn=tr.pn||2;
            tpl[1]=(m*2).toFixed(2);
            tr.cost.innerHTML=tpl.join('');
            tpl[1]=(m*pn).toFixed(2);
            tr.back.innerHTML=tpl.join('');
            */
        };
        var ns = this;
        clearTimeout(ns.batAddTimer);
        ns.batAddTimer = setTimeout(function() {
            ns.showMoneyInfo()
        }, 48);
    },

    getShowCount: function() {//合计可见行
        var i = 0;
        each($id('tr', this.box), function() {
            if (this.style.display != 'none') i++
        });
        return i
    },

    showMoneyInfo: function() {//统计并显示投入回报数据
        var count = 0, cost = 0, back = 0, tmp = {}, m;
        each($id('tr', this.box), function() {
            if (this.style.display == 'none') return;
            count++;
            var m = 1/*this.edit.value*/, pn = this.pn || 2;
            cost += m * 2;
            if (!(this.zid in tmp)) tmp[this.zid] = [];
            tmp[this.zid].push(pn * m);
        });
        var min = 0, max = 0;
        for (var k in tmp) {
            min += Math.min.apply(Math, tmp[k]);
            max += Math.max.apply(Math, tmp[k]);
        };
        back = min == max ? ('￥' + min.toFixed(2)) : (('￥' + min.toFixed(2)) + '-' + ('￥' + max.toFixed(2)));
        this.result.count.innerHTML = count;
        /*
        this.result.cost.innerHTML='￥'+cost.toFixed(2);
        this.result.back.innerHTML=back;
        */
    },
    onHide: function() { }
}

var ggAdmin = {
    ggType:
    ['r2c1', 'r3c1,r3c3,r3c4', 'r4c1,r4c4,r4c5,r4c6,r4c11', 'r5c1,r5c5,r5c6,r5c10,r5c16,r5c20,r5c26', 'r6c1,r6c6,r6c7,r6c15,r6c20,r6c22,r6c35,r6c42,r6c50,r6c57', 'r7c1,r7c7,r7c8,r7c21,r7c35,r7c120', 'r8c1,r8c8,r8c9,r8c28,r8c56,r8c70,r8c247'],
    ggValue: eval('(' + $id('jsonggtype').value + ')'),
    curType: false,
    vsData: [],

    init: function(typeListBox) {
        var Data = [];
        var list = $id('input', typeListBox);
        this.wager = 0;
        this.typeList = [];
        for (var i = list.length; i--; )
            this.typeList.push(list[i]);
        var NS = this;
        each(this.typeList, function() {
            this.onclick = function() {//切换过关选择
                var wagerCount = 0;
                var DanData = selectTable.getDanData();
                var Data = selectTable.getData();
                var sa = [], sb = [];
                var b0 = $id('dpUnsureDan').value;
                var b1 = 0;

                for (var j = 0; j < DanData.length; j++) { if (DanData[j] == 0) { sa.push(Data[j]); } else { sb.push(Data[j]); b1++; } }
                wagerCount = myCalc(this.value.replace('串', '_'), sa, sb, b0, b1);
                if (this.checked) { ggAdmin.wager += wagerCount; ggAdmin.curType += "," + this.value; } else { ggAdmin.wager -= wagerCount; ggAdmin.curType = ggAdmin.curType.replace(this.value, '').replace(',,', ',') }

                if (ggAdmin.curType.indexOf(',') == 0) {
                    ggAdmin.curType = ggAdmin.curType.substring(1);
                }

                if (ggAdmin.curType.lastIndexOf(',') + 1 == ggAdmin.curType.length && ggAdmin.curType.length > 0) {
                    ggAdmin.curType = ggAdmin.curType.substring(0, ggAdmin.curType.length - 1);
                }

                ggAdmin.show(ggAdmin.wager);
            }
        })
    },

    setGGType: function(m, n) {//显示可用过关方式
        n = n > selectTable.MaxMatch ? selectTable.MaxMatch : n;
        this.curType = "";
        var useType = "";
        if (n > 1) {
            for (var i = n; i > m; i--)
                if (i > 0) {
                useType += this.ggType.slice(i - 2, i - 1) + ",";
            }
            useType = useType.substring(0, useType.length - 1);
        }

        if (n > 2) {
            $(".cmore").show();
        }
        else {
            $(".cmore").hide();
        }

        each(this.typeList, function() {
            var use = useType.indexOf(this.id) + 1;
            if (this.checked) { this.checked = (use > 0); if (use > 0) { ggAdmin.curType += "," + this.value; } }
            this.parentNode.style.display = use ? '' : 'none';
        })

        if (ggAdmin.curType.indexOf(',') == 0) {
            ggAdmin.curType = ggAdmin.curType.substring(1);
        }

        if (ggAdmin.curType.lastIndexOf(',') + 1 == ggAdmin.curType.length && ggAdmin.curType.length > 0) {
            ggAdmin.curType = ggAdmin.curType.substring(0, ggAdmin.curType.length - 1);
        }

        ggAdmin.show(ggAdmin.wager);
    },

    update: function(data) {
        this.vsData = data;
        this._onChange()
    },

    show: function(wagerCount) {
        var bs = $id('buybs').value;
        $id('zhushu').value = $id('showCount').innerHTML = wagerCount;
        $id('beishu').value = bs;
        $id('showMoney').innerHTML = '￥' + ($id('totalmoney').value = (wagerCount * bs * 2).toFixed(2));
        ResetShare();
        var Share = parseInt($id('tb_Share').value);
        var BuyShare = parseInt($id('tb_BuyShare').value);
        var AssureShare = parseInt($id('tb_AssureShare').value);

        var SumMoney = (bs * 2 * wagerCount).toFixed(2);
        var ShareMoney = (SumMoney / Share).toFixed(2);

        var AssureMoney = (AssureShare * ShareMoney).toFixed(2);
        var BuyMoney = (BuyShare * ShareMoney).toFixed(2);

        $id('lab_ShareMoney').innerText = ShareMoney;
        $id('lab_AssureMoney').innerText = AssureMoney;
        $id('lab_BuyMoney').innerText = BuyMoney;

        var maxSP = [], baseLine, map = infoWin.__pnMap, danSP = [], sp1 = [];
        each(selectTable.box.rows, function() {//查找预选表
            if (this.style.display == 'none') return;
            sp = [], sp1 = [];
            var match = this;
            var Dan = false;

            each(this.getElementsByTagName('INPUT'), function() {
                if (this.checked) Dan = true;
            }, this.getElementsByTagName('INPUT').length - 1, this.getElementsByTagName('INPUT').length);

            each($id('label', this), function(i) {//预选SP
                baseLine = $id('vs' + match.zid);
                if (this.style.display != 'none') {
                    var tmpPn = infoWin.__getPn__(baseLine, map[i]);

                    if (Dan) { sp1.push(parseFloat(tmpPn || 2.00)); }
                    else { sp.push(parseFloat(tmpPn || 2.00)); }
                }
            });

            if (Dan) { var Dan = Math.max.apply(Math, sp1); danSP.push(Dan); }
            else { var max = Math.max.apply(Math, sp); maxSP.push(max); }
        });    //查找结束

        danSP.sort(function(a, b) { return b - a });
        $id('maxmoney').innerText = "0.00";
        var DanCount = parseInt($id('dpUnsureDan').value) || 0;

        if (ggAdmin.curType.length > 0) {
            var ggtypes = ggAdmin.curType.split(',');
            var InfoDetail;
            var subHtml = "";
            var SumMoney = 0, m = 0;

            for (var i = 0; i < ggtypes.length; i++) {
                if (ggtypes[i] != '') {
                    var sub = typeMap[ggtypes[i]], t, r, p;
                    for (var k in sub) {
                        m = 0;
                        while (danSP.length >= DanCount + m) {
                            t = $CL(danSP, danSP.length - m, true, []);
                            for (var n = 0; n < t.length; n++) {
                                r = $CL(maxSP, parseInt(ggtypes[i]), true, t[n]);
                                for (var q = 0; q < r.length; q++) {
                                    p = $CL(r[q], parseInt(k), true, []);
                                    for (var j = 0; j < p.length; j++) {
                                        xsum = $Tran.apply(null, p[j]);
                                        SumMoney += xsum * bs * 2;
                                    }
                                }
                            }
                            m++;
                        }
                    };
                }
            }

            $id('maxmoney').innerText = SumMoney.toFixedFix(2);
        }
    },

    _onChange: function() {
        if (this.vsData.length < 2) {
            this.data = this.data = 0;
        } else {
            this.data = get_puy_Count(this.vsData, this.curType);
        }
        this.onChange(this.data)

    },

    setSecrecyLevel: function(ggSecrecyLevel) {
        var list = $id('input', ggSecrecyLevel);
        this.SecrecyLevelList = [];
        for (var i = list.length; i--; )
            this.SecrecyLevelList.push(list[i]);
        each(this.SecrecyLevelList, function() {
            this.onclick = function() {//切换过关选择
                $id('SecrecyL').value = this.value;
            }
        })
    },

    onChange: function() { }
};

/*
IO Event
------------------------------------------------------------------------------------------*/
function setUserEvent(vs, sel) {
    var hideCount = $id('hideCount'),
  showAll = $id('showAllTeam'),
  showRq = $id('showRq'),
  showNoRq = $id('showNoRq'),
  lgList = $id('lgList'),
  selectAllBtn = $id('selectAllBtn'),
  selectOppBtn = $id('selectOppBtn'),
  allMatch = $id('input', 'lgList');
    vs.onHide = function(tr, count) {
        count = count || vs.getHideCount();
        hideCount.innerHTML = count;
        if (tr == undefined && showRq)
            showRq.checked = showNoRq.checked = true;
    };

    showAll.onclick = function() {//恢复
        vs.showAll();
        hideCount.innerHTML = 0;
        if (lgList) {
            each(lgList.getElementsByTagName('input'), function() {
                this.checked = true;
            });
        }
    };

    if (showRq)
        showRq.onclick = function() {//显隐让球
            vs.showRQ('rq', this.checked);
            hideCount.innerHTML = vs.getHideCount();
        };

    if (showNoRq)
        showNoRq.onclick = function() {//显隐非让球
            vs.showRQ('nrq', this.checked);
            hideCount.innerHTML = vs.getHideCount();
        };

    if (lgList)
        lgList.onclick = function(e) {//按赛事选
            e = fixEvent(e);
            var src = e.target;
            if (src.tagName.toLowerCase() != 'input') return;
            vs.showByName(attr(src, 'm'), src.checked)
        };

    if (selectAllBtn)
        selectAllBtn.onclick = function() {//全选
            for (var i = allMatch.length; i--; ) allMatch[i].checked = true;
            vs.showAll()
        };

    if (selectOppBtn)
        selectOppBtn.onclick = function() {//反选
            for (var i = allMatch.length; i--; ) {
                allMatch[i].checked = !allMatch[i].checked;
                vs.showByName(attr(allMatch[i], 'm'), allMatch[i].checked)
            }
        }

    var op_col = $id('op_col'), vstable = $id('table_vs');
    function colChange() {// 切换欧赔与平均列
        switch (this.value) {
            case '99家平均': updatePn.init('../xml/Average99.xml?t=' + (new Date()).getTime().toString(), 2000); return;
            case '威廉希尔': updatePn.init('../xml/Willhill.xml?t=' + (new Date()).getTime().toString(), 2000); return;
            case '立博': updatePn.init('../xml/Lad.xml?t=' + (new Date()).getTime().toString(), 2000); return;
            case 'Bet365': updatePn.init('../xml/Bet365.xml?t=' + (new Date()).getTime().toString(), 2000); return;
            case '澳门彩票': updatePn.init('../xml/Macau.xml?t=' + (new Date()).getTime().toString(), 2000); return;
            default: return;
        }
    };
    if (op_col) {
        on(op_col, 'change', colChange);
    };

    function timechange() {
        switch (this.value) {
            case '0': $(".end_time").show(); $(".match_time").hide(); return;
            case '1': $(".match_time").show(); $(".end_time").hide(); return;
            default: return;
        }
    }

    var select_time = $id('select_time')
    if (select_time) {
        on(select_time, 'change', timechange);
    };
}
/*
auto update
*/
updatePn = {
    init: function(url, time) {
        var NS = this;
        http(url, function(h, x) {
            each($id('m', x), function(i) {
                NS.parse(this)
            })
        })
    },
    parse: function(line) {
        var map = document.getElementById('vs' + attr(line, 'id'));
        if (map) {
            var New = [];
            New[0] = attr(line, 'win');
            New[1] = attr(line, 'draw');
            New[2] = attr(line, 'lost');
            attrinnerText(map, 'odds', New)
        }
    },
    onChange: function(vsTr, xmlNode) { }
};

/*

*/
function $CL(arr, n, noLC, arrDan) {//从arr中取n个组合，然后
    var r = [], sum = 0;
    n = n - arrDan.length;
    (function f(t, a, n) {
        if (n == 0) return r.push(t);
        if (a.length < 1) return;
        for (var i = 0,
		l = a.length - n; i <= l; i++) {
            f(t.concat(a[i]), a.slice(i + 1), n - 1);
        }
    })(arrDan, arr, n);
    if (noLC) return r; //返回一个组合数组[[],[],[]]
    for (var i = r.length; i--; ) sum += get1(r[i]); //计算每个组合的连乘的和
    return sum;
};

function $CLDT(arr_D, arr_T, n, noLC) {//从arr中取n个组合，然后
    var r = [], sum = 0;
    (function f(t, a, n) {
        if (n == 0) return r.push(t);
        for (var i = 0,
		l = a.length - n; i <= l; i++) {
            f(t.concat(a[i]), a.slice(i + 1), n - 1);
        }
    })(arr_D, arr, n);
    if (noLC) return r; //返回一个组合数组[[],[],[]]
    for (var i = r.length; i--; ) sum += get1(r[i]); //计算每个组合的连乘的和
    return sum;
};

function get1(arr) { return $Tran.apply(null, arr) || 0 }; //对数组进行连乘
function get2(arr) { return get1(arr) + $CL(arr, arr.length - 1, true, []) }; //一个基本串+(n-1)个串一[跳到CL进行拆分然后求单组的和]
function get3(arr) { return get2(arr) + $CL(arr, arr.length - 2, true, []) };
function get4(arr) { return get3(arr) + $CL(arr, arr.length - 3, true, []) };
function get5(arr) { return get4(arr) + $CL(arr, arr.length - 4, true, []) };
function get6(arr) { return $CL(arr, arr.length - 1, true, []) };
function get7(arr) { return $CL(arr, arr.length - 2, true, []) };
function get8(arr) { return $CL(arr, arr.length - 3, true, []) };
function get9(arr) { return $CL(arr, arr.length - 4, true, []) };
function get10(arr) { return $CL(arr, arr.length - 5, true, []) };
function get11(arr) { return $CL(arr, arr.length - 6, true, []) };

function get_puy_Count(arr/*intArray*/, ggType) {
    switch (ggType) {
        case '2串1': case '3串1': case '4串1': case '5串1': case '6串1': case '7串1': case '8串1': return get1(arr); //串1直接连乘
        case '3串4': case '4串5': case '5串6': case '6串7': case '7串8': case '8串9': return get2(arr);
        case '4串11': case '5串16': case '6串22': return get3(arr);
        case '5串26': case '6串42': return get4(arr);
        case '6串57': return get5(arr);
        case '3串3': case '4串4': case '5串5': case '6串6': case '7串7': case '8串8': return get6(arr);
        case '4串6': case '7串21': case '8串28': return get7(arr);
        case '5串10': case '6串20': case '7串35': case '8串56': return get8(arr);
        case '5串20': return get8(arr) + get7(arr);
        case '6串15': case '8串70': return get9(arr);
        case '6串35': return get8(arr) + get9(arr);
        case '6串50': return get8(arr) + get7(arr) + get9(arr);
        case '7串120': return get10(arr) + get9(arr) + get8(arr) + get7(arr) + get6(arr) + get1(arr);
        case '8串247': return get11(arr) + get10(arr) + get9(arr) + get8(arr) + get7(arr) + get6(arr) + get1(arr);
        default: return 0;
    }
};
var typeMap = {
    //串1,n5
    '2串1': { '2串1': 1 },
    '3串1': { '3串1': 1 },
    '4串1': { '4串1': 1 },
    '5串1': { '5串1': 1 },
    '6串1': { '6串1': 1 },
    '7串1': { '7串1': 1 },
    '8串1': { '8串1': 1 },
    //n21
    '3串3': { '2串1': 3 },
    '3串4': { '3串1': 1, '2串1': 3 },
    '4串6': { '2串1': 6 },
    '4串11': { '4串1': 1, '3串1': 4, '2串1': 6 },
    '5串10': { '2串1': 10 },
    '5串20': { '2串1': 10, '3串1': 10 },
    '5串26': { '5串1': 1, '4串1': 5, '3串1': 10, '2串1': 10 },
    '6串15': { '2串1': 15 },
    '6串35': { '2串1': 15, '3串1': 20 },
    '6串50': { '2串1': 15, '3串1': 20, '4串1': 15 },
    '6串57': { '6串1': 1, '5串1': 6, '4串1': 15, '3串1': 20, '2串1': 15 },
    '4串4': { '3串1': 4 },
    '4串5': { '4串1': 1, '3串1': 4 },
    '5串16': { '5串1': 1, '4串1': 5, '3串1': 10 },
    '6串20': { '3串1': 20 },
    '6串42': { '6串1': 1, '5串1': 6, '4串1': 15, '3串1': 20 },
    '5串5': { '4串1': 5 },
    '5串6': { '5串1': 1, '4串1': 5 },
    '6串22': { '6串1': 1, '5串1': 6, '4串1': 15 },
    '6串6': { '5串1': 6 },
    '6串7': { '6串1': 1, '5串1': 6 },
    '7串7': { '6串1': 7 },
    '7串8': { '6串1': 7, '7串1': 1 },
    '7串21': { '5串1': 21 },
    '7串35': { '4串1': 35 },
    '7串120': { '2串1': 21, '3串1': 35, '4串1': 35, '5串1': 21, '6串1': 7, '7串1': 1 },
    '8串8': { '7串1': 8 },
    '8串9': { '7串1': 8, '8串1': 1 },
    '8串28': { '6串1': 28 },
    '8串56': { '5串1': 56 },
    '8串70': { '4串1': 70 },
    '8串247': { '2串1': 28, '3串1': 56, '4串1': 70, '5串1': 56, '6串1': 28, '7串1': 8, '8串1': 1 }
};

/*
infoWin
*/
function showMoneyTable(who, countDan, countTuo) {
    var maxSP = [], minSP = [], baseLine, map = infoWin.__pnMap, maxdanSP = [], mindanSP = [], sp1 = [];
    each(selectTable.box.rows, function() {//查找预选表
        if (this.style.display == 'none') return;
        sp = [], sp1 = [];
        var match = this;
        var Dan = false;

        each(this.getElementsByTagName('INPUT'), function() {
            if (this.checked) Dan = true;
        }, this.getElementsByTagName('INPUT').length - 1, this.getElementsByTagName('INPUT').length);

        each($id('label', this), function(i) {//预选SP
            baseLine = $id('vs' + match.zid);
            if (this.style.display != 'none') {
                var tmpPn = infoWin.__getPn__(baseLine, map[i]);

                if (Dan) {
                    sp1.push(parseFloat(tmpPn || 2.00)); //最少2.0
                }
                else {
                    sp.push(parseFloat(tmpPn || 2.00)); //最少2.0
                }
            }
        });

        if (Dan) {
            var minDan = Math.min.apply(Math, sp1), maxDan = Math.max.apply(Math, sp1);
            mindanSP.push(minDan); //最小SP表
            maxdanSP.push(maxDan); //最大SP表
        }
        else {
            var min = Math.min.apply(Math, sp), max = Math.max.apply(Math, sp);
            minSP.push(min); //最小SP表
            maxSP.push(max); //最大SP表
        }
    });     //查找结束

    var ggtypes = infoWin.ggTypeSet.split(',');
    var subtypes = "";
    for (var i = 0; i < ggtypes.length; i++) {
        if (ggtypes[i] != '') {
            var sub = typeMap[ggtypes[i]];
            for (var k in sub) {
                if (parseInt(k) <= (countDan + countTuo)) {
                    subtypes += k + ",";
                }
            };
        }
    }

    if (subtypes.length > 0) { subtypes = subtypes.substring(0, subtypes.length - 1); }
    var types = getNum(subtypes.split(','));

    minSP.sort(function(a, b) { return a - b });
    maxSP.sort(function(a, b) { return b - a });
    mindanSP.sort(function(a, b) { return a - b });
    maxdanSP.sort(function(a, b) { return b - a });

    var DanCount = parseInt($id('dpUnsureDan').value) || 0;

    while (mindanSP.length > parseInt(countDan)) {
        mindanSP.pop(); maxdanSP.pop();
    }

    while (maxSP.length > parseInt(countTuo)) {
        maxSP.pop(); minSP.pop();
    }

    subHtml = "";
    var Sum = 0, SumMoney = 0, bs = $id('buybs').value, InfoDetail;
    var tempSP = [], tempdanSP = [], temphtml = '';

    if (who == 'max') {
        tempSP = maxSP, tempdanSP = maxdanSP;
    }
    else {
        var tempSP = minSP, tempdanSP = mindanSP;
    }

    for (var i = types.length - 1; i >= 0; i--) {
        subHtml += "<tr>";
        subHtml += '<td width="50px" class="trt">' + types[i][0] + '</td>';
        var sumcount = 0, money = 0;

        InfoDetail = infoWin.Detail(tempSP, types[i][0], bs, tempdanSP);

        subHtml += '<td width="350px" class="trt">' + InfoDetail.Sum + '注</td>';
        subHtml += '<td  class="td_line vcenter trt">' + InfoDetail.val + '</td>';
        subHtml += "</tr>";
        subHtml += InfoDetail.html;
        Sum += InfoDetail.Sum;
        SumMoney += InfoDetail.val;
    }

    var Html = {
        n: parseInt(countDan) + parseInt(countTuo),
        body3: subHtml,
        zs: Sum,
        sumMoney: SumMoney.toFixedFix(2)
    };

    $id("popMatchDetail").style.display = "";
    $("#popMatchDetail").html(replace(infoWin.html, Html));
};
infoWin = {
    tpl: [
        '<table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox"><tr class="trt"><th class="td_line">赛事编号</th><th class="td_line">比赛</th><th class="td_line">您的选择(奖金)</th><th class="td_line">最小奖金值</th><th class="td_line">最大奖金值</th><th class="td_line">胆码</th></td></tr>{$body}<tr><td colspan=6 class="td_line td_line4">{$foot}</td></tr></table>',
        '<table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox"><tr class="trt"><td rowspan="2">命中场数</td><td colspan={$head2col}>中奖注数</td><td rowspan=2>倍数</td><td colspan=2>奖金范围</td></tr><tr class="trt">{$ggTypeSub}<td>最大奖金</td><td class="td_line">最小奖金</td></tr>{$body2}<tr><td colspan={$foot2col} class="td_line td_line4 red">注：奖金预测值为投注时即时奖金，最终奖金以出票时刻的奖金为准</td></tr></table>',
        '<table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox" id=\"popMatchDetail\" style=\"display:none;\"><tr class="trt"><td colspan=3>中{$n}场 中奖明细</td></tr>{$body3}<tr><td>合计</td><td>{$zs}注</td><td class="td_line vcenter"><span class="bold">{$sumMoney}</span>元</td></tr></table>',
        ],
    html: '<tr class="trt"><td colspan=3>中{$n}场 中奖明细</td></tr>{$body3}<tr><td>合计</td><td>{$zs}注</td><td class="td_line vcenter"><span class="bold">{$sumMoney}</span>元</td></tr>',
    init: function(btn, win, title, body, moneyUI) {
        this.btn = $id(btn);
        if (!this.btn) return;
        var ns = this;
        this.btn.onclick = function() {
            ns.onShow()
        };
        drag($id(title), $id(win));
        this.win = $id(win);
        this.body = $id(body);
        this.moneyUI = $id(moneyUI)
    },
    show: function(selector) {
        var pos = getClientSize();
        var x = parseInt((pos.width - 560) / 2) + 'px';
        var y = parseInt((pos.height - 560) / 3) + pos.y + 'px';
        this.win.style.display = '';
        this.win.style.top = y;
        this.win.style.left = x;
        var Html = this.getDetails(selector.box.rows);
        this.body.innerHTML = replace(this.tpl.join(''), Html);
    },
    __pnTitle: ['胜', '平', '负'],
    __pnMap: ['win', 'draw', 'lost'],
    __getPn__: function(tr, pnName) {
        return parseFloat(attr(tr, pnName)) || 2.00
    },
    getDetails: function(rows) {//生成明细数据
        var data = [], baseLine, tmp, map = this.__pnMap, cnmap = this.__pnTitle;
        var max = [], count = 0, maxSP = [], minSP = [], ns = this, maxdanSP = [], mindanSP = [], sp1 = [];
        each(rows, function() {//查找预选表
            if (this.style.display == 'none') return;
            var Dan = false;
            count++; //场数
            baseLine = $id('vs' + this.zid);
            tmp = [], sp = [], sp1 = [];

            each(this.getElementsByTagName('INPUT'), function() {
                if (this.checked) Dan = true;
            }, this.getElementsByTagName('INPUT').length - 1, this.getElementsByTagName('INPUT').length);

            each($id('label', this), function(i) {//预选SP
                if (this.style.display != 'none') {
                    var tmpPn = ns.__getPn__(baseLine, map[i]);
                    tmp.push(cnmap[i] + '(' + tmpPn.toFixed(2) + ') ')

                    if (Dan) {
                        sp1.push(parseFloat(tmpPn || 2.00)); //最少2.0
                    }
                    else {
                        sp.push(parseFloat(tmpPn || 2.00)); //最少2.0
                    }
                }
            });

            if (Dan) {
                var minDan = Math.min.apply(Math, sp1), maxDan = Math.max.apply(Math, sp1);
                mindanSP.push(minDan); //最小SP表
                maxdanSP.push(maxDan); //最大SP表

                data.push('<tr><td>' + delHtml(this.cells[0].innerHTML)
					+ '</td><td>' + delHtml(baseLine.cells[1].innerHTML)
					+ '</td><td>' + tmp.join('') + '</td><td>' + parseFloat(minDan).toFixed(2) + '</td><td>' + parseFloat(maxDan).toFixed(2) + '</td><td style="color:#f00;"  class="td_line">√</td></tr>');
            }
            else {
                var min = Math.min.apply(Math, sp), max = Math.max.apply(Math, sp);
                minSP.push(min); //最小SP表
                maxSP.push(max); //最大SP表

                data.push('<tr><td>' + delHtml(this.cells[0].innerHTML)
					+ '</td><td>' + delHtml(baseLine.cells[1].innerHTML)
					+ '</td><td>' + tmp.join('') + '</td><td>' + parseFloat(min).toFixed(2) + '</td><td>' + parseFloat(max).toFixed(2) + '</td><td  class="td_line">×</td></tr>');
            }
        }); //查找结束
        var subHtml = '', subZs = '';
        var len = 0;
        var ggtypes = this.ggTypeSet.split(',');
        var subtypes = "";

        for (var i = 0; i < ggtypes.length; i++) {
            if (ggtypes[i] != '') {
                var sub = typeMap[ggtypes[i]];
                for (var k in sub) {
                    subtypes += k + ",";
                };
            }
        }

        if (subtypes.length > 0) { subtypes = subtypes.substring(0, subtypes.length - 1); }
        var types = getNum(subtypes.split(','));

        types.sort();
        minSP.sort(function(a, b) { return a - b });
        maxSP.sort(function(a, b) { return b - a });
        mindanSP.sort(function(a, b) { return a - b });
        maxdanSP.sort(function(a, b) { return b - a });

        subHtml = "";
        len = types.length;

        for (var i = types.length - 1; i >= 0; i--) {
            subHtml += '<td>' + types[i][0] + '</td>';
        }
        var body = "";
        var InfoDetail, bs = $id('buybs').value;
        var DanCount = parseInt($id('dpUnsureDan').value) || 0;

        for (var i = data.length; i > 0; i--) {
            var m = 0, a = 0, b = 0, Dan = 0;
            while (mindanSP.length >= m) {
                var tempmindanSP = [], tempminSP = [], tempmaxdanSP = [], tempmaxSP = [];
                var tempmindanSP = tempmindanSP.concat(mindanSP); tempminSP = tempminSP.concat(minSP), tempmaxdanSP = tempmaxdanSP.concat(maxdanSP), tempmaxSP = tempmaxSP.concat(maxSP);
                a = m;
                while (tempminSP.length + tempmindanSP.length > i) {
                    if (a > 0) { tempmindanSP.pop(); tempmaxdanSP.pop(); a--; } else { if (tempminSP.length == 0) { tempmindanSP.pop(); tempmaxdanSP.pop(); } tempminSP.pop(); tempmaxSP.pop(); }
                }

                if ((Dan == tempmindanSP.length && DanCount > 0) || ((tempmindanSP.length + tempminSP.length) < parseInt(types[0][0]))) { break; }

                if (DanCount == 0) { body += '<tr><td>中' + i + '场</td>'; } else {
                    body += '<tr><td>中' + i + '场[胆中' + tempmindanSP.length + '场][拖中' + tempminSP.length + '场]</td>';
                }
                var minInfo = 0, maxInfo = 0, InfoDetail;
                for (var j = types.length - 1; j >= 0; j--) {
                    InfoDetail = this.SP2Money(tempminSP, types[j][0], bs, tempmindanSP);
                    minInfo += InfoDetail.val;

                    InfoDetail = this.SP2Money(tempmaxSP, types[j][0], bs, tempmaxdanSP);
                    maxInfo += InfoDetail.val;

                    body += '<td>' + InfoDetail.Sum + '</td>';
                }

                body += '<td>' + this.bs + '</td><td>' + maxInfo.toFixedFix(2) + '元<span id="changeMax" style="color:#0066CC;cursor:pointer;" onclick="showMoneyTable(\'max\', \'' + tempmindanSP.length + '\', \'' + tempminSP.length + '\')">[明细]</span></td><td class="td_line">'
				+ minInfo.toFixedFix(2) + '元<span id="changeMin" style="color:#0066CC;;cursor:pointer;" onclick="showMoneyTable(\'min\', \'' + tempmindanSP.length + '\', \'' + tempminSP.length + '\')">[明细]</span></td></tr>';

                m++;
                Dan = tempmindanSP.length;
            }
        }

        return {
            body: data.join('\n'), //表1
            foot: '过关方式：<font color=red>' + this.ggTypeSet + '</font> 倍数：<font color=red>' + this.bs + '倍</font> 方案总金额：<font color=red>' + this.moneyUI.innerHTML + '</font>元',
            ggTypeSub: subHtml, //表2
            head2col: len,
            body2: body,
            foot2col: len + 5,
            n: count
        };
        function delHtml(x) {
            return x.toString().replace(/<[^>]+>/g, '')
        }
    },
    SP2Money: function(tempSP, ggType, BS, tempdanSP) {//计算最大或者最小奖金并构建详情表
        var sum = 0, r, p, t, BS = BS || 1, DanCount = parseInt($id('dpUnsureDan').value) || 0, minInfo = 0;
        if (tempdanSP.length < DanCount) { DanCount = tempdanSP.length };
        var ggtypes = this.ggTypeSet.split(',');
        for (var k = 0; k < ggtypes.length; k++) {
            if (ggtypes[k] != '') {
                var sub = typeMap[ggtypes[k]];
                for (var type in sub) {
                    if (type == ggType) {
                        var L = 0;
                        while (L + DanCount <= tempdanSP.length) {
                            t = $CL(tempdanSP, DanCount + L, true, []);
                            for (var n = 0; n < t.length; n++) {
                                r = $CL(tempSP, parseInt(ggtypes[k]), true, t[n]);
                                if (r.length < 1) { r[0] = t[n].concat(tempSP); }
                                for (var q = 0; q < r.length; q++) {
                                    p = $CL(r[q], parseInt(type), true, []);
                                    for (var s = 0; s < p.length; s++) {
                                        xsum = $Tran.apply(null, p[s]);
                                        minInfo += xsum * BS;
                                    }
                                    sum += p.length;
                                }
                            }
                            L++;
                        }
                    }
                };
            }
        }

        return { val: (minInfo * BS).toFixedFix(2) * 2, Sum: sum }
    },
    Detail: function(tempSP, ggType, BS, tempdanSP) {//计算最大或者最小奖金并构建详情表
        var sum = 0, r, p, t, BS = BS || 1, DanCount = parseInt($id('dpUnsureDan').value) || 0, minInfo = 0, Html = [];
        if (tempdanSP.length < DanCount) { DanCount = tempdanSP.length };
        var ggtypes = this.ggTypeSet.split(',');
        for (var k = 0; k < ggtypes.length; k++) {
            if (ggtypes[k] != '') {
                var sub = typeMap[ggtypes[k]];
                for (var type in sub) {
                    if (type == ggType) {
                        var L = 0;
                        while (L + DanCount <= tempdanSP.length) {
                            t = $CL(tempdanSP, DanCount + L, true, []);
                            for (var n = 0; n < t.length; n++) {
                                r = $CL(tempSP, parseInt(ggtypes[k]), true, t[n]);
                                if (r.length < 1) { r[0] = t[n].concat(tempSP); }
                                for (var q = 0; q < r.length; q++) {
                                    p = $CL(r[q], parseInt(type), true, []);
                                    Html.push('<tr><td></td><td style="text-align:left;width:320px;padding-left:5px;"><p class="td_width">');
                                    for (var s = 0; s < p.length; s++) {
                                        xsum = $Tran.apply(null, p[s]);
                                        minInfo += xsum * BS;
                                        Html.push(p[s].join(' × ') + ' × ' + BS + '倍 × 2元= <b style="color:#0066CC">' + (xsum * BS).toFixedFix(2) * 2 + '</b> 元<br/>');
                                    }
                                    Html.push('</p></td><td class="td_line"></td></tr>');
                                    sum += p.length;
                                }
                            }
                            L++;
                        }
                    }
                };
            }
        }

        return { val: (minInfo * BS).toFixedFix(2) * 2, html: Html.join(''), Sum: sum }
    },
    onShow: function() { },
    Matchc: function(len, m) {
        return (function(n1, n2, j, i, n) {
            for (; j <= m; ) {
                n2 *= j++;
                n1 *= i--
            }
            return n1 / n2
        })(1, 1, 1, len, len)
    }
};

function ResetShare() {
    var multiple = parseInt($id('buybs').value);
    multiple = multiple == 0 ? 1 : multiple;
    var SumNum = parseInt($id('showCount').innerHTML);
    var SumMoney = Math.round(multiple * 2 * SumNum, 2);
    if (Math.round(SumMoney) < 1) { SumMoney == 1; }

    if ($id('Scheme_join').checked && SumMoney > 1) {
        var SchemeScola = eval('(' + $id('SchemeSchemeBonusScalec').value + ')');

        var bScalec = parseFloat(SchemeScola["bScalec"]);
        var LScaleMoney = parseInt(SchemeScola["LScaleMoney"]);
        var LScale = parseFloat(SchemeScola["LScale"]);
        var UScaleMoney = parseInt(SchemeScola["UScaleMoney"]);
        var UpperScale = parseFloat(SchemeScola["UpperScale"]);

        var bScalec = 0;

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

        var Share = $id('tb_Share').value;

        if ((Math.round((SumMoney / Share) * 100) / 100 != SumMoney / Share) || Share == 1) {
            $id('tb_Share').value = Math.round(SumMoney) < 1 ? 1 : Math.round(SumMoney);
        }

        Share = $id('tb_Share').value;
        var BuyShare = Math.round(Share * bScalec) < 1 ? 1 : Math.round(Share * bScalec);

        if (BuyShare > Share) {
            BuyShare = Share;
        }

        if (parseInt($id('tb_BuyShare').value) > Share || parseInt($id('tb_BuyShare').value) < BuyShare) { $id('tb_BuyShare').value = BuyShare };

    } else {
        $id('tb_Share').value = "1";
        $id('tb_BuyShare').value = "1";
    }
}

function myCalc(PassType, sa, sb, b0, b1) {
    if (sb.length == 0)
        return calc(PassType, sa, sb); else {
        var wcount = 0; for (var k = b0; k <= b1; k++) {
            var bm = combinArray(sb, k); for (var j = 0; j < bm.length; j++)
            { wcount += calc(PassType, sa, bm[j]); }
        }
        return wcount;
    }
}

function calc(PassType, sa, sb) {
    var WagerCount = 0; var t_list = PassType.split("_"); var pc = parseInt(t_list[0], 10);

    var AbsCount = sb.length; var len = pc - AbsCount;
    if (len == 0 && AbsCount > 0) {
        var pm = new Array(pc); for (var P = 0; P < sb.length; P++) {
            var AbsVoteC = sb[P]; for (var k = 0; k < pc; k++) {
                if (pm[k] == 0 || pm[k] == null)
                { pm[k] = AbsVoteC; break; }
            }
        }
        var pstr = pm.slice(0, pc).join(","); eval("WagerCount += calcuteWC(PassType," + pstr + ");");
    }
    else {
        var arr = new Array(); for (var I = 0; I < sa.length; I++)
        { arr[arr.length] = I; }
        var w = combinArray(arr, len); for (var I = 0; I < w.length; I++) {
            var splitArr = w[I]; var pm = new Array(pc); for (var k = 0; k < pc; k++)
            { var d = splitArr[k]; pm[k] = splitArr[k] != null ? sa[d] : 0; }
            if (AbsCount > 0) {
                for (var P = 0; P < sb.length; P++) {
                    var AbsVoteC = sb[P]; for (var k = 0; k < pc; k++) {
                        if (pm[k] == 0 || pm[k] == null)
                        { pm[k] = AbsVoteC; break; }
                    }
                }
            }
            var pstr = pm.slice(0, pc).join(","); eval("WagerCount += calcuteWC(PassType," + pstr + ");");
        }
    }
    return WagerCount;
}

function combinArray(arr, len) {
    var Re = new Array(); arr.sort();
    if (arr.length < len || len < 1)
    { return Re; }
    else if (arr.length == len)
    { Re[0] = arr; return Re; }

    if (len == 1) {
        for (var I = 0; I < arr.length; I++)
        { Re[I] = new Array(); Re[I][0] = arr[I]; }
        return Re;
    }

    if (len > 1) {
        for (var I = 0; I < arr.length; I++) {
            var arr_b = new Array();
            for (var J = 0; J < arr.length; J++) {
                if (J > I)
                    arr_b[arr_b.length] = arr[J];
            }
            var s = combinArray(arr_b, len - 1);
            if (s.length > 0) {
                for (var K = 0; K < s.length; K++)
                { var p = s[K]; p[p.length] = arr[I]; Re[Re.length] = p; }
            }
        }
    }
    return Re;
}

function calcuteWC(passtype, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) {
    var re = 0; a = a == null ? 0 : parseInt(a, 10); b = b == null ? 0 : parseInt(b, 10); c = c == null ? 0 : parseInt(c, 10); d = d == null ? 0 : parseInt(d, 10); e = e == null ? 0 : parseInt(e, 10); f = f == null ? 0 : parseInt(f, 10); g = g == null ? 0 : parseInt(g, 10); h = h == null ? 0 : parseInt(h, 10); i = i == null ? 0 : parseInt(i, 10); j = j == null ? 0 : parseInt(j, 10); k = k == null ? 0 : parseInt(k, 10); l = l == null ? 0 : parseInt(l, 10); m = m == null ? 0 : parseInt(m, 10); n = n == null ? 0 : parseInt(n, 10); o = o == null ? 0 : parseInt(o, 10); switch (passtype) {
        case "1_1": re = a; break; case "2_1": re = a * b; break; case "2_3": re = (a + 1) * (b + 1) - 1; break; case "3_1": re = a * b * c; break; case "3_3": re = a * b + a * c + b * c; break; case "3_4": re = a * b * c + a * b + a * c + b * c; break; case "3_7": re = (a + 1) * (b + 1) * (c + 1) - 1; break; case "4_1": re = a * b * c * d; break; case "4_4": re = a * b * c + a * b * d + a * c * d + b * c * d; break; case "4_5": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) - (a * (b + c + d + 1) + b * (c + d + 1) + (c + 1) * (d + 1)); break; case "4_6": re = a * b + a * c + a * d + b * c + b * d + c * d; break; case "4_11": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) - (a + b + c + d + 1); break; case "4_15": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) - 1; break; case "5_1": re = a * b * c * d * e; break; case "5_5": re = a * b * c * d + a * b * c * e + a * b * d * e + a * c * d * e + b * c * d * e; break; case "5_6": re = a * b * c * d * e + a * b * c * d + a * b * c * e + a * b * d * e + a * c * d * e + b * c * d * e; break; case "5_10": re = a * b + a * c + a * d + a * e + b * c + b * d + b * e + c * d + c * e + d * e; break; case "5_16": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) - (a * (b + c + d + e + 1) + b * (c + d + e + 1) + c * (d + e + 1) + (d + 1) * (e + 1)); break; case "5_20": re = a * b * c + a * b * d + a * b * e + a * c * d + a * c * e + a * d * e + b * c * d + b * c * e + b * d * e + c * d * e
+ a * b + a * c + a * d + a * e + b * c + b * d + b * e + c * d + c * e + d * e; break; case "5_26": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) - (a + b + c + d + e + 1); break; case "5_31": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) - 1; break; case "6_1": re = a * b * c * d * e * f; break; case "6_6": re = a * b * c * d * e + a * b * c * d * f + a * b * c * e * f + a * b * d * e * f + a * c * d * e * f + b * c * d * e * f; break; case "6_7": re = a * b * c * d * e * f + a * b * c * d * e + a * b * c * d * f + a * b * c * e * f + a * b * d * e * f + a * c * d * e * f + b * c * d * e * f; break; case "6_15": re = a * b + a * c + a * d + a * e + a * f + b * c + b * d + b * e + b * f + c * d + c * e + c * f + d * e + d * f + e * f; break; case "6_20": re = a * b * c + a * b * d + a * b * e + a * b * f + a * c * d + a * c * e + a * c * f + a * d * e + a * d * f + a * e * f + b * c * d + b * c * e + b * c * f + b * d * e + b * d * f + b * e * f + c * d * e + c * d * f + c * e * f + d * e * f; break; case "6_22": re = a * b * c * d * e * f + a * b * c * d * e + a * b * c * d * f + a * b * c * e * f + a * b * d * e * f + a * c * d * e * f + b * c * d * e * f
+ a * b * c * d + a * b * c * e + a * b * c * f + a * b * d * e + a * b * d * f + a * b * e * f + a * c * d * e + a * c * d * f + a * c * e * f + a * d * e * f
+ b * c * d * e + b * c * d * f + b * c * e * f + b * d * e * f + c * d * e * f; break; case "6_35": re = a * b * c + a * b * d + a * b * e + a * b * f + a * c * d + a * c * e + a * c * f + a * d * e + a * d * f + a * e * f + b * c * d + b * c * e + b * c * f + b * d * e + b * d * f + b * e * f + c * d * e + c * d * f + c * e * f + d * e * f + a * b + a * c + a * d + a * e + a * f + b * c + b * d + b * e + b * f + c * d + c * e + c * f + d * e + d * f + e * f; break; case "6_42": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) * (f + 1) - (a * (b + c + d + e + f + 1) + b * (c + d + e + f + 1) + c * (d + e + f + 1) + d * (e + f + 1) + (e + 1) * (f + 1)); break; case "6_50": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) * (f + 1) - (a + b + c + d + e + f + 1) - (a * b * c * d * e + a * b * c * d * f + a * b * c * e * f + a * b * d * e * f + a * c * d * e * f + b * c * d * e * f + a * b * c * d * e * f); break; case "6_57": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) * (f + 1) - (a + b + c + d + e + f + 1); break; case "6_63": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) * (f + 1) - 1; break; case "7_1": re = a * b * c * d * e * f * g; break; case "7_7": re = a * b * c * d * e * f + a * b * c * d * e * g + a * b * c * d * f * g + a * b * c * e * f * g + a * b * d * e * f * g + a * c * d * e * f * g + b * c * d * e * f * g; break; case "7_8": re = a * b * c * d * e * f * g + a * b * c * d * e * f + a * b * c * d * e * g + a * b * c * d * f * g + a * b * c * e * f * g + a * b * d * e * f * g + a * c * d * e * f * g + b * c * d * e * f * g; break; case "7_21": re = a * b * c * d * e + a * b * c * d * f + a * b * c * d * g + a * b * c * e * f
+ a * b * c * e * g + a * b * c * f * g + a * b * d * e * f + a * b * d * e * g
+ a * b * d * f * g + a * b * e * f * g + a * c * d * e * f + a * c * d * e * g
+ a * c * d * f * g + a * c * e * f * g + a * d * e * f * g + b * c * d * e * f
+ b * c * d * e * g + b * c * d * f * g + b * c * e * f * g + b * d * e * f * g
+ c * d * e * f * g; break; case "7_35": re = a * b * c * d + a * b * c * e + a * b * c * f + a * b * c * g + a * b * d * e + a * b * d * f
+ a * b * d * g + a * b * e * f + a * b * e * g + a * b * f * g + a * c * d * e + a * c * d * f
+ a * c * d * g + a * c * e * f + a * c * e * g + a * c * f * g + a * d * e * f + a * d * e * g
+ a * d * f * g + a * e * f * g + b * c * d * e + b * c * d * f + b * c * d * g + b * c * e * f
+ b * c * e * g + b * c * f * g + b * d * e * f + b * d * e * g + b * d * f * g + b * e * f * g
+ c * d * e * f + c * d * e * g + c * d * f * g + c * e * f * g + d * e * f * g; break; case "7_120": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) * (f + 1) * (g + 1) - (a + b + c + d + e + f + g + 1); break; case "8_1": re = a * b * c * d * e * f * g * h; break; case "8_8": re = a * b * c * d * e * f * g + a * b * c * d * e * f * h + a * b * c * d * e * g * h
+ a * b * c * d * f * g * h + a * b * c * e * f * g * h + a * b * d * e * f * g * h
+ a * c * d * e * f * g * h + b * c * d * e * f * g * h; break; case "8_9": re = a * b * c * d * e * f * g * h + a * b * c * d * e * f * g + a * b * c * d * e * f * h
+ a * b * c * d * e * g * h + a * b * c * d * f * g * h + a * b * c * e * f * g * h
+ a * b * d * e * f * g * h + a * c * d * e * f * g * h + b * c * d * e * f * g * h; break; case "8_28": re = a * b * c * d * e * f + a * b * c * d * e * g + a * b * c * d * e * h + a * b * c * d * f * g
+ a * b * c * d * f * h + a * b * c * d * g * h + a * b * c * e * f * g + a * b * c * e * f * h
+ a * b * c * e * g * h + a * b * c * f * g * h + a * b * d * e * f * g + a * b * d * e * f * h
+ a * b * d * e * g * h + a * b * d * f * g * h + a * b * e * f * g * h + a * c * d * e * f * g
+ a * c * d * e * f * h + a * c * d * e * g * h + a * c * d * f * g * h + a * c * e * f * g * h
+ a * d * e * f * g * h + b * c * d * e * f * g + b * c * d * e * f * h + b * c * d * e * g * h
+ b * c * d * f * g * h + b * c * e * f * g * h + b * d * e * f * g * h + c * d * e * f * g * h; break; case "8_56": re = a * b * c * d * e + a * b * c * d * f + a * b * c * d * g + a * b * c * d * h + a * b * c * e * f
+ a * b * c * e * g + a * b * c * e * h + a * b * c * f * g + a * b * c * f * h + a * b * c * g * h
+ a * b * d * e * f + a * b * d * e * g + a * b * d * e * h + a * b * d * f * g + a * b * d * f * h
+ a * b * d * g * h + a * b * e * f * g + a * b * e * f * h + a * b * e * g * h + a * b * f * g * h
+ a * c * d * e * f + a * c * d * e * g + a * c * d * e * h + a * c * d * f * g + a * c * d * f * h
+ a * c * d * g * h + a * c * e * f * g + a * c * e * f * h + a * c * e * g * h + a * c * f * g * h
+ a * d * e * f * g + a * d * e * f * h + a * d * e * g * h + a * d * f * g * h + a * e * f * g * h
+ b * c * d * e * f + b * c * d * e * g + b * c * d * e * h + b * c * d * f * g + b * c * d * f * h
+ b * c * d * g * h + b * c * e * f * g + b * c * e * f * h + b * c * e * g * h + b * c * f * g * h
+ b * d * e * f * g + b * d * e * f * h + b * d * e * g * h + b * d * f * g * h + b * e * f * g * h
+ c * d * e * f * g + c * d * e * f * h + c * d * e * g * h + c * d * f * g * h + c * e * f * g * h
+ d * e * f * g * h; break; case "8_70": re = a * b * c * d + a * b * c * e + a * b * c * f + a * b * c * g + a * b * c * h + a * b * d * e
+ a * b * d * f + a * b * d * g + a * b * d * h + a * b * e * f + a * b * e * g + a * b * e * h
+ a * b * f * g + a * b * f * h + a * b * g * h + a * c * d * e + a * c * d * f + a * c * d * g
+ a * c * d * h + a * c * e * f + a * c * e * g + a * c * e * h + a * c * f * g + a * c * f * h
+ a * c * g * h + a * d * e * f + a * d * e * g + a * d * e * h + a * d * f * g + a * d * f * h
+ a * d * g * h + a * e * f * g + a * e * f * h + a * e * g * h + a * f * g * h + b * c * d * e
+ b * c * d * f + b * c * d * g + b * c * d * h + b * c * e * f + b * c * e * g + b * c * e * h
+ b * c * f * g + b * c * f * h + b * c * g * h + b * d * e * f + b * d * e * g + b * d * e * h
+ b * d * f * g + b * d * f * h + b * d * g * h + b * e * f * g + b * e * f * h + b * e * g * h
+ b * f * g * h + c * d * e * f + c * d * e * g + c * d * e * h + c * d * f * g + c * d * f * h
+ c * d * g * h + c * e * f * g + c * e * f * h + c * e * g * h + c * f * g * h + d * e * f * g
+ d * e * f * h + d * e * g * h + d * f * g * h + e * f * g * h; break; case "8_247": re = (a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) * (f + 1) * (g + 1) * (h + 1) - (a + b + c + d + e + f + g + h + 1); break; case "9_1": re = a * b * c * d * e * f * g * h * i; break; case "10_1": re = a * b * c * d * e * f * g * h * i * j; break; case "11_1": re = a * b * c * d * e * f * g * h * i * j * k; break; case "12_1": re = a * b * c * d * e * f * g * h * i * j * k * l; break; case "13_1": re = a * b * c * d * e * f * g * h * i * j * k * l * m; break; case "14_1": re = a * b * c * d * e * f * g * h * i * j * k * l * m * n; break; case "15_1": re = a * b * c * d * e * f * g * h * i * j * k * l * m * n * o; break; default: break;
    }
    return re;
}

function opendate(id) {
    try {
        if (document.getElementById('d_' + id).style.display == 'none') {
            document.getElementById('d_' + id).style.display = '';
            document.getElementById('img_' + id).className = 's_hidden';
            document.getElementById('img_' + id).innerHTML = '点击隐藏';
            document.getElementById('img_' + id).alt = '隐藏';
        } else {
            document.getElementById('d_' + id).style.display = 'none';
            document.getElementById('img_' + id).className = 's_add';
            document.getElementById('img_' + id).innerHTML = '点击展开';
            document.getElementById('img_' + id).alt = '展开';
        }
    } catch (e) { };
}

function openclose(id) {
    if (document.getElementById('pltr_' + id).style.display == 'none') {
        document.getElementById('pltr_' + id).style.display = '';
        document.getElementById('img_' + id).src = 'Images/btn_sp.gif';
        document.getElementById('img_' + id).alt = '隐藏选项';
    } else {
        document.getElementById('pltr_' + id).style.display = 'none';
        document.getElementById('img_' + id).src = 'Images/btn_spadd.gif';
        document.getElementById('img_' + id).alt = '展开选项';

    }
}

function getNum(a) {
    var r = [], o = {};
    for (var i = 0, l = a.length; i < l; i++) {
        o[a[i]] ? o[a[i]]++ : o[a[i]] = 1;
    }
    for (var j in o) r.push([j, o[j]]);
    return r;
}

function nTabs(thisObj, Num) {
    if (thisObj.className == "active") return;
    var tabObj = thisObj.parentNode.id;
    var tabList = document.getElementById(tabObj).getElementsByTagName("li");
    for (i = 0; i < tabList.length; i++) {
        if (i == Num) {
            thisObj.className = "active";
            document.getElementById(tabObj + "_Content" + i).style.display = "block";

            $("#gTab_Content0").find("[type='checkbox']:checkbox").each(function() {
                if (this.type == "checkbox" && this.checked) {
                    $(this).attr("checked", false);
                    $(this).click();
                    $(this).attr("checked", false);
                }
            });
        } else {
            tabList[i].className = "normal";
            document.getElementById(tabObj + "_Content" + i).style.display = "none";

            $("#gTab_Content1").find("[type='checkbox']:checkbox").each(function() {
                if (this.type == "checkbox" && this.checked) {
                    $(this).attr("checked", false);
                    $(this).click();
                    $(this).attr("checked", false);
                }
            });
        }
    }
}
