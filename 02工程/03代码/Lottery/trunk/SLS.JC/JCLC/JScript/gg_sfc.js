selectTable.max = 4;
selectTable.DanMa = 7;
selectTable.mapTr = function(tr, chk) {
    return document.getElementById('sel' + tr.zid)
};
selectTable.del = function(tr, chk) {
    var mapTr = this.mapTr(tr, chk);
    if (mapTr) {
        var chks = mapTr.getElementsByTagName('INPUT');
        this.showCheckBox(chks, chk.value, false);
        if (this.isAllCancel(mapTr)) mapTr.hide();
        this._onChange()
    }
};
selectTable.add = function(tr, chk) {
    var mapTr = this.mapTr(tr, chk); //映射行
    if (this.getShowCount() == this.max) {
        if (!mapTr || mapTr.style.display == 'none') {
            each(tr.getElementsByTagName('INPUT'), function() { this.checked = false }, 1);
            return msg('单个方案最多只能选择' + this.max + '场比赛进行投注');
        }
    };
    var newTr = this.getNewTr(tr, chk, mapTr);
    this.box.appendChild(newTr);
    this.reSort();
    this._onChange()
};
selectTable.getNewTr = function(tr, chk, node) {//添加行
    var tmp = node, NS = this;
    if (!node) {
        tmp = this.tpl.cloneNode(true)
        var tds = tmp.getElementsByTagName('TD');
        tmp.id = 'sel' + tr.zid;
        tmp.zid = attr(tr, 'zid'); //zid
        tmp.spf = parseInt(chk.value);
        tmp.preFix = attr(tr, 'zid');
        tmp.date = attr(tr, 'pdate');
        tmp.pname = attr(tr, 'pname');
        tmp.pn = parseFloat(attr(tr, ["", "win", "lost"][tmp.spf])) || 0; //赔率
        tmp.style.display = '';
        tds[0].lastChild.innerHTML = this.unHTML(tr.cells[0].innerHTML);
        tds[1].innerHTML = this.unHTML(tr.cells[4].innerHTML);
        tmp.hide = function() { this.style.display = 'none' };
        tmp.show = function() { this.style.display = '' };
    } else {
        this.show(tr, chk)
    };
    var chks = tmp.getElementsByTagName('INPUT');
    this.showCheckBox(chks, chk.value, true)
    chks[0].onclick = function() {//隐藏行
        each(tmp.getElementsByTagName('INPUT'), function() {
            this.onclick()
        }, 1);
        this.checked = true;
        tmp.hide();
    };
    chks[chks.length - 1].onclick = function() {//隐藏行
        var Dan = selectTable.getDanCount();
        var maxDan = selectTable.getShowCount() - 1;
        maxDan = maxDan > selectTable.DanMa ? selectTable.DanMa : maxDan;
        if ((Dan > selectTable.DanMa || Dan >= selectTable.getShowCount()) && Dan > 0) {
            this.checked = false;
            return msg('单个方案最多只能选择' + maxDan + '个胆码进行投注');
        }

        selectTable.UnsureDanMa(Dan);
        ggAdmin.onChange();
    };

    each(chks, function(i) {//check事件
        this.onclick = function() {
            this.parentNode.style.display = 'none';
            NS.onHide(tmp, this.value);
            if (NS.isAllCancel(tmp)) tmp.hide();
            NS._onChange()//发起onchange事件
        }
    }, 1, 13);

    // 核心方法
    var ems = tmp.getElementsByTagName('em');

    each(ems, function(i) {//check事件
        this.onclick = function() {
            //alert("xx");
            this.parentNode.style.display = 'none';
            // $(this).prev().prev().hide();
            NS.onHide(tmp, $(this).prev().val());
            if (NS.isAllCancel(tmp)) tmp.hide();
            NS._onChange()//发起onchange事件
        }
    }, 0);

    return tmp
};
selectTable.showCheckBox = function(chks, value, checked) {//显示胜平负
    var index = [0, 7, 1, 8, 2, 9, 3, 10, 4, 11, 5, 12, 6][value];
    var o = chks[index];
    o.checked = checked;
    o.parentNode.style.display = checked ? '' : 'none';
};
selectTable.isAllCancel = function(tr) {
    var test = true;
    each(tr.getElementsByTagName('INPUT'), function() { if (this.parentNode.style.display != 'none') return test = false }, 1, 13);
    each(tr.getElementsByTagName('INPUT'), function() { this.checked = false; }, 13, 14);
    return test
};
selectTable.hideAll = function() {//全部关闭
    each($id('tr', this.box), function() {
        if (this.style.display != 'none') {
            this.getElementsByTagName('INPUT')[0].onclick()
        }
    });
};

//模糊胆下拉
selectTable.UnsureDanMa = function(danmaNum) {
    var dpUnsureDan = $id("dpUnsureDan");
    if (dpUnsureDan == null) return;
    var passCount = selectTable.DanMa;
    $id("spnUnsureDan").style.display = (danmaNum > 1) ? "" : "none";
    for (var i = dpUnsureDan.options.length - 1; i >= 0; i--) { dpUnsureDan.options[i] = null; }
    //选择了8场比赛，当选了7个胆时仅提供模糊设胆六个以下，否则没有多选选项
    if (danmaNum > 0) {
        if (danmaNum <= passCount) {
            for (var i = danmaNum; i > 0; i--) {
                dpUnsureDan.options.add(new Option("至少含" + i + "胆", i));
            }
        }
        ggAdmin.setGGType(danmaNum > selectTable.DanMa ? selectTable.DanMa : danmaNum, this.getShowCount());
    }
};


selectTable.getDanCount = function() {//合计可见行
    var i = 0;
    each($id('tr', this.box), function() {
        if (this.style.display == 'none') return;
        each(this.getElementsByTagName('INPUT'), function() {
            if (this.checked) i++
        }, 13, 14);
    });
    return i
};

selectTable.getDanData = function() {//合计可见行
    var data = [];
    each($id('tr', this.box), function() {
        if (this.style.display == 'none') return;
        var t = 0;
        each(this.getElementsByTagName('INPUT'), function() {
            if (this.checked) t++
        }, 13, 14);
        data.push(t)
    });

    this.data = data;
    return data
};

selectTable.getData = function() {
    var data = [];
    each($id('tr', this.box), function() {
        if (this.style.display == 'none') return;
        var t = 0;
        each(this.getElementsByTagName('LABEL'), function() {
            if (this.style.display != 'none') t++
        });
        data.push(t)
    });
    this.data = data;
    return data;
};
selectTable.showMoneyInfo = function() { };
selectTable._onChange = function() {
    var NS = this;
    clearTimeout(NS.timer);
    NS.timer = setTimeout(function() { NS.onChange(NS.getData()) }, 10); //连续变化只保留最后一次有效
};
selectTable.onChange = function() { };
/*
begin
*/
table_vs.__getChks__ = function(tr) {
    var zid = attr(tr, 'zid');
    if (!zid) return; //not main line
    var header = $id('input', tr)[0];
    var pnTable = $id('pltr_' + zid);
    if (!pnTable) return { head: header, opts: [] };
    tr.pnTr = pnTable;
    var opts = [], all = [];
    each([tr, pnTable], function() {
        var chks = $id('INPUT', this);
        var len = chks.length;
        if (len < 1) return false;
        var body = [];
        if (this.getAttribute('c') != null) {
            each(chks, function(i) {
                body.push(this)
            }, 0, -1);
        } else {
            each(chks, function(i) {
                body.push(this)
            }, 1, -1);
        }
        all = all.concat(body);
        if (chks.length <= 1) { //已截止的比赛
            opts = null;
        } else {
            opts.push({
                options: body,
                allSelect: chks[len - 1]
            })
        }
    });
    return {
        head: header,
        opts: opts,
        options: all
    }
};
table_vs.__setRow__ = function(tr) {
    var chks = this.__getChks__(tr);
    if (!chks) return;

    var NS = this;
    this.list.push(tr);
    tr.hide = function() {//行隐藏
        this.style.display = 'none';
        if (this.pnTr) {
            this.pnTr.style.display = 'none';
            if ($id('img_' + tr.zid)) $id('img_' + tr.zid).src = '/images/abtn_pl22.gif'
        };
        NS.onHide(this, this.__hideCount++)
    };
    tr.show = function() {//行显示
        tr.style.display = '';
        if (this.pnTr) {
            this.pnTr.style.display = '';
            if ($id('img_' + tr.zid)) $id('img_' + tr.zid).src = '/images/abtn_pl2.gif'
        };
        NS.onHide(this, this.__hideCount--)
    };
    chks.head.onclick = function() {//隐藏[sender:chk,influence:tr]
        this.checked = true;
        tr.hide()
    };

    if (chks.opts == null || chks.opts.length == 0) return; // 过期比赛

    tr.options = chks.opts;
    tr.zid = attr(tr, 'zid');
    tr.id = 'vs' + tr.zid;

    tr.updatePn = function(data) {//更新折率
        NS.__updatePn__(this, data)
    };

    each(chks.opts, function(i, item) {//三行
        var opts = this.options; //单行选项
        var allOpt = this.allSelect;
        for (var i = opts.length; i--; ) {//单选
            opts[i].checked = false; //初始不选中
            opts[i].onclick = function(e) {
                allOpt.checked = NS.isAllSelect(opts);
                NS.onSelect(tr, this)
                e = e || window.event;
                if (e.preventDefault) e.stopPropagation();
            };
            opts[i].cancel = function() {//手动取消选择
                this.checked = false;
                allOpt.checked = NS.isAllSelect(opts)
            }
            opts[i].parentNode.onclick = AutoClick;
        };
        allOpt.checked = false; //初始不选中
        allOpt.parentNode.onclick = AutoClick;
        allOpt.onclick = function(e) {//全选(末项)
            for (var i = opts.length; i--; ) {
                if (opts[i].checked != this.checked) {
                    opts[i].checked = this.checked;
                    NS.onSelect(tr, opts[i])
                }
            }
            e = e || window.event;
            if (e.preventDefault) e.stopPropagation(); //Firefox下事件冒泡
        }
    })

};
table_vs.setCheckBox = function(zid, spf) {//外部操作checkbox
    var tr = $id('pltr_' + zid);
    if (tr) {
        var chks = tr.getElementsByTagName('INPUT');
        each($id('input', tr), function() {//取消客胜
            if (parseInt(this.value, 10) == parseInt(spf, 10)) {
                this.cancel();
                this.parentNode.className = 'sp_bg';
                return false
            }
        })
        each($id('input', tr.previousSibling), function() {//取消主胜
            if (parseInt(this.value, 10) == parseInt(spf, 10)) {
                this.cancel();
                this.parentNode.className = 'sp_bg';
                return false
            }
        })
    }
};
table_vs.showAll = function() {//显示所有
    var list = this.list, el;
    for (var i = list.length; i--; ) {
        list[i].show();
    }
    this.onHide()
}

table_vs.show = function(wagerCount) {
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

    // Loong Add        
    $("#lab_BuyScale").text((BuyShare / Share * 100).toFixed(1) + "%");
    $("#lab_AssureScale").text((AssureShare / Share * 100).toFixed(1) + "%");
};

table_vs.setSecrecyLevel = function(ggSecrecyLevel) {
    var list = $id('input', ggSecrecyLevel);
    this.SecrecyLevelList = [];
    for (var i = list.length; i--; )
        this.SecrecyLevelList.push(list[i]);
    each(this.SecrecyLevelList, function() {
        this.onclick = function() {//切换过关选择
            $id('SecrecyL').value = this.value;
        }
    })
};

function App() {
    table_vs.__valToChk = [0, 1, 2, 3, 4, 5, 6, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6];

    table_vs.init('table_vs');
    setUserEvent(table_vs, selectTable);
    selectTable.init('selectTeams', 'row_tpl')

    selectTable.onHide = function(tr, spf) {
        table_vs.setCheckBox(tr.zid, spf);
    };
    table_vs.onSelect = function(tr, chk) {
        chk.checked ? selectTable.add(tr, chk) : selectTable.del(tr, chk)
        chk.parentNode.className = chk.checked ? selectedColor : 'sp_bg';
    };

    ggAdmin.init('ggList');
    table_vs.setSecrecyLevel('ggSecrecyLevel');
    selectTable.onChange = function(data/*Array[int,int(1-3)]*/) {
        var n = data.length;
        if (n != ggAdmin.vsData.length)//如果队列数量有变,切换过关方式
        {
            ggAdmin.setGGType(selectTable.getDanCount() == 0 ? 0 : selectTable.getDanCount(), this.getShowCount() > 0 ? this.getShowCount() : n);
            selectTable.UnsureDanMa(selectTable.getDanCount());
        }
        ggAdmin.update(data);
    };

    ggAdmin._onChange = function() {
        this.data = this.vsData.length < 2 ? 0 : get_puy_Count(this.vsData, this.curType);
        this.onChange(this.data)
    },

  ggAdmin.onChange = function(data) {//合计
      var DanData = selectTable.getDanData();
      var Data = selectTable.getData();
      var wagerCount = 0;

      ggAdmin.curType = "";

      each($id('input', 'ggList'), function() {
          var sa = [], sb = [];
          var b0 = $id('dpUnsureDan').value;
          var b1 = 0;

          for (var j = 0; j < DanData.length; j++) { if (DanData[j] == 0) { sa.push(Data[j]); } else { sb.push(Data[j]); b1++; } }
          if (this.checked) { wagerCount += myCalc(this.value.replace('串', '_'), sa, sb, b0, b1); ggAdmin.curType += this.value + ","; }
      })

      if (ggAdmin.curType.length > 0) {
          //var ggtypes = ggAdmin.curType.indexOf(',') >= 0 ? ggAdmin.curType.substring(0, ggAdmin.curType.length - 1).split(',') : ggAdmin.curType.split(',');
          ggAdmin.curType = ggAdmin.curType.substring(0, ggAdmin.curType.length - 1);
      }

      ggAdmin.wager = wagerCount;
      ggAdmin.show(wagerCount);
  };

    infoWin.__pnTitle = ['主胜1-5', '主胜6-10', '主胜11-15', '主胜16-20', '主胜21-25', '主胜26+',
	                   '客胜1-5', '客胜6-10', '客胜11-15', '客胜16-20', '客胜21-25', '客胜26+'];
    infoWin.__pnMap = ['hw1t5', 'hw6t10', 'hw11t15', 'hw16t20', 'hw21t25', 'hw26',
	                   'gw1t5', 'gw6t10', 'gw11t15', 'gw21t25', 'gw16t20', 'gw26'];
    infoWin.__getPn__ = function(tr, pnName) {
        var odds = attr(tr, 'odds');
        var aOdds = odds.split(',');
        return parseFloat(aOdds[index_of(infoWin.__pnMap, pnName)]) || 2.00;
    }

    infoWin.init('lookDetails', 'popWindow', 'popWindowTitle', 'popContent', 'showMoney');
    infoWin.onShow = function() {
        if (selectTable.getShowCount() < 2) return msg('请最少选择两场比赛!');
        if (!ggAdmin.curType) return msg('请选择过关方式!');
        this.ggTypeSet = ggAdmin.curType;
        this.bs = $id('buybs').value;
        this.show(selectTable)
    }

    intInputChange($id('buybs'), function() {//倍数输入框
        if (isNaN(ggAdmin.data)) return;
        ggAdmin.onChange(ggAdmin.data)
    });

    intInputChange($id('tb_SchemeBonusScale'), function() {//佣金
        var o_tb_SchemeBonusScale = $id('tb_SchemeBonusScale');
        if (isNaN($id('tb_SchemeBonusScale').value)) return msg('请输入数字');

        var SchemeBonusScale = parseInt(o_tb_SchemeBonusScale.value);
        o_tb_SchemeBonusScale.value = SchemeBonusScale;

        if (SchemeBonusScale < 0) o_tb_SchemeBonusScale.value = 0;
        if (SchemeBonusScale > 10) o_tb_SchemeBonusScale.value = 10;
    });

    intInputChange($id('tb_Share'), function() {//份数
        var o_tb_Share = $id('tb_Share');
        var Share = parseInt(o_tb_Share.value);
        var OK = false;

        o_tb_Share.value = Share;

        if (Share < 0) { return msg("输入的份数非法。"); OK = false; }
        else if (Share == 1) { OK = true; }
        else {
            if (Share > 1) {
                var multiple = parseInt($id('buybs').value);
                var SumNum = parseInt($id('showCount').innerHTML);
                var SumMoney = multiple * 2 * SumNum;
                var ShareMoney = SumMoney / Share;
                var ShareMoney2 = Math.round(ShareMoney * 100) / 100;

                if (ShareMoney == ShareMoney2) OK = true;
                if (ShareMoney < 1) { OK = false; }
            }
        }

        if (!OK) {
            if (confirm("份数为 0 或者不能除尽，将产生误差，并且金额不能小于 1 元。按“确定”重新输入，按“取消”自动更正为 1 份，请选择。")) { o_tb_Share.focus(); return; }
            else { Share = 1; o_tb_Share.value = 1; }
        }

        $id('tb_AssureShare').value = "0";

        if (isNaN(ggAdmin.data)) return;
        ggAdmin.onChange(ggAdmin.data)
    });

    intInputChange($id('tb_BuyShare'), function() {//认购
        var BuyShare = parseInt($id('tb_BuyShare').value);
        var Share = parseInt($id('tb_Share').value);
        var AssureShare = parseInt($id('tb_AssureShare').value);

        if ((BuyShare < 1) || (BuyShare > Share)) {
            if (confirm("购买份数不能为 0 以及大于总份数同时份数必须为整数。按“确定”重新输入，按“取消”自动更正为 " + Share + " 份，请选择。")) {
                $id('tb_BuyShare').focus(); return;
            }
            else { $id('tb_BuyShare').value = Share; BuyShare = Share; }
        }

        if ((BuyShare + AssureShare) > Share) {
            AssureShare = Share - BuyShare;
            msg("购买和保底份数大于总份数，保底份数自动调整为 " + AssureShare + "。");
            $id('tb_AssureShare').value = AssureShare;
            return;
        }

        if (isNaN(ggAdmin.data)) return;
        ggAdmin.onChange(ggAdmin.data)
    });

    intInputChange($id('tb_AssureShare'), function() {//保底
        var Share = parseInt($id('tb_Share').value);
        var AssureShare = parseInt($id('tb_AssureShare').value);
        var BuyShare = parseInt($id('tb_BuyShare').value);

        if (AssureShare < 0) {
            msg("输入的保底份数非法。");
            $id('tb_AssureShare').value = "0";
            return;
        }

        if ((Share == 1) && (AssureShare > 0)) {
            msg("此方案只分为 1 份，不能保底。");
            $id('tb_AssureShare').value = "0";
            return;
        }
        if (AssureShare > (Share - 1)) {
            var AutoAssureShare = Math.round(Share / 2);
            if ((AutoAssureShare + BuyShare) > Share)
                AutoAssureShare = Share - BuyShare;
            if (confirm("保底份数不能大于和等于总份数。按“确定”重新输入，按“取消”自动更正为 " + AutoAssureShare + " 份，请选择。")) {
                $id('tb_AssureShare').focus();
                return;
            }
            else {
                $id('tb_AssureShare').value = AutoAssureShare;
                AssureShare = AutoAssureShare;
            }
        }
        if ((BuyShare + AssureShare) > Share) {
            BuyShare = Share - AssureShare;
            msg("购买份数与保底份数和大于总份数，购买份数自动调整为 " + BuyShare + " 份。");
            $id('tb_BuyShare').value = BuyShare;
        }

        if (isNaN(ggAdmin.data)) return;
        ggAdmin.onChange(ggAdmin.data)
    });

    if ($id('clearAllSelect') == null) return;
    $id('clearAllSelect').onclick = function() {
        selectTable.hideAll()
    };


    if ($id('Scheme_Buy') == null) return;
    $id('Scheme_Buy').onclick = function() {
        $id('trShowJion').style.display = $id('Scheme_join').checked == true ? "" : "none";
        ggAdmin.onChange(ggAdmin.data);

    };

    if ($id('Scheme_join') == null) return;
    $id('Scheme_join').onclick = function() {
        $id('trShowJion').style.display = $id('Scheme_join').checked == true ? "" : "none";
        ggAdmin.onChange(ggAdmin.data);
    };
};
App();

/**
* 返回数组中特定元素的下标
*/
function index_of(arr, val) {
    for (var i = 0, l = arr.length; i < l; i++) {
        if (arr[i] == val) return i;
    }
    return -1;
}