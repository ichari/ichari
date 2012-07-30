table_vs.__updatePn__=function (tr,data){
    var d=0,t=0,old=attr(tr,'odds');
    if(old)old=old.split(',');
    each(this.__getChks__(tr).options,function (i){
        var span=getNext(this);
        if(!span)return;
        if(isNaN(parseInt(data[i])))return;
        var d=data[i]-old[i];
        if(d!=0){
            var t=parseFloat(data[i]);
            span.innerHTML=t==0?'':(data[i]+(d>0?'<span style="color:#F00;font-weight:normal;">↑</span>': '<span style="color:#090;font-weight:normal;">↓<span>'));
            span.title=(d>0?'+':'')+d.toFixed(2);
        }
    });
   attr(tr,'odds',data)
};
updatePn.parse=function (line){
    var map=document.getElementById('vs'+attr(line,'id'));
    if(map)
        map.updatePn([attr(line,'s0'),attr(line,'s1'),attr(line,'s2'),attr(line,'s3'),attr(line,'s4'),attr(line,'s5'),attr(line,'s6'),attr(line,'s7')])
};
/*
reover
*/
selectTable.max = 10;
selectTable.DanMa = 5;
selectTable.MaxMatch = 6;
selectTable.mapTr=function (tr,chk){
    return document.getElementById('sel'+tr.zid)
};
selectTable.del = function(tr, chk) {
    var mapTr = this.mapTr(tr, chk);
    if (mapTr) {
        var chks = mapTr.getElementsByTagName('INPUT');
        this.showCheckBox(chks, chk.value, false);
        if (this.isAllCancel(mapTr)) mapTr.hide();
        this._onChange();
        ggAdmin.onChange();
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
    this._onChange();
    ggAdmin.onChange();
};
selectTable.getNewTr = function(tr, chk, node) {//添加行
    var tmp = node, NS = this;
    if (!node) {
        tmp = this.tpl.cloneNode(true)
        var tds = tmp.getElementsByTagName('TD');
        tmp.id = 'sel' + tr.zid;
        tmp.zid = attr(tr, 'zid'); //zid
        tmp.spf = parseInt(chk.value); //310
        tmp.preFix = attr(tr, 'mid');
        tmp.date = attr(tr, 'pdate');
        tmp.pname = attr(tr, 'pname');
        tmp.pn = parseFloat(attr(tr, ["lost", "draw", "", "win"][tmp.spf])) || 0; //赔率
        tmp.style.display = '';
        tds[0].lastChild.innerHTML = this.unHTML(tr.cells[0].innerHTML);
        tds[1].innerHTML = this.unHTML(tr.cells[this.__namePos[0]].innerHTML);
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
    }, 1, 9);

    // 核心方法
    var ems = tmp.getElementsByTagName('em');

    each(ems, function(i) {//check事件
        this.onclick = function() {
            if (this.parentNode == null || this.parentNode == "undefined") {
                return;
            }
            this.parentNode.style.display = 'none';
            NS.onHide(tmp, $(this).prev().val());
            if (NS.isAllCancel(tmp)) tmp.hide();
            NS._onChange()//发起onchange事件
        }
    }, 0, 9);
    return tmp
};
selectTable.__valToChkPos=[1,2,3,4,5,6,7,8];
selectTable.showCheckBox=function (chks,value,checked){//显示胜平负
    var index=this.__valToChkPos[value],o=chks[index];
    o.checked=checked;
    o.parentNode.style.display=checked?'':'none';
};
selectTable.isAllCancel=function (tr){
    var test=true;
    each(tr.getElementsByTagName('INPUT'), function() { if (this.parentNode.style.display != 'none') return test = false }, 1, 9);
    each(tr.getElementsByTagName('INPUT'), function() { this.checked = false; }, 9, 10);
    return test
};
selectTable.hideAll=function (){//全部关闭
    each($id('tr',this.box),function (){
        if(this.style.display!='none'){
            this.getElementsByTagName('INPUT')[0].onclick()
        }
    });
};


//模糊胆下拉
selectTable.UnsureDanMa = function(danmaNum) {
    var dpUnsureDan = $id("dpUnsureDan");
    if (dpUnsureDan == null) return;
    var passCount = 5;
    $id("spnUnsureDan").style.display = (danmaNum > 1) ? "" : "none";
    for (var i = dpUnsureDan.options.length - 1; i >= 0; i--) { dpUnsureDan.options[i] = null; }
    //选择了8场比赛，当选了7个胆时仅提供模糊设胆六个以下，否则没有多选选项
    if (danmaNum > 0) {
        if (danmaNum <= passCount) {
            for (var i = danmaNum; i > 0; i--) {
                dpUnsureDan.options.add(new Option("至少含" + i + "胆", i));
            }
        }
        ggAdmin.setGGType(danmaNum > 5 ? 5 : danmaNum, this.getShowCount());
    }
};

selectTable.getDanCount = function() {//合计可见行
    var i = 0;
    each($id('tr', this.box), function() {
        if (this.style.display == 'none') return;
        each(this.getElementsByTagName('INPUT'), function() {
            if (this.checked) i++
        }, 9, 10);
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
        }, 9, 10);
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
    return data
};

selectTable.getData=function (){
    var data=[];
    each($id('tr',this.box),function (){
        if(this.style.display=='none')return;
        var t=0;
        each(this.getElementsByTagName('LABEL'),function (){
            if(this.style.display!='none')t++
        });
        data.push(t)
    });
    this.data=data;
    return data
};
selectTable.showMoneyInfo=function(){};
selectTable._onChange=function(){
   var NS=this;
   clearTimeout(NS.timer);
   NS.timer=setTimeout(function (){NS.onChange(NS.getData())},10);//连续变化只保留最后一次有效
};
selectTable.onChange=function(){};
//
infoWin.__pnTitle=[0,1,2,3,4,5,6,'7+'];
infoWin.__pnMap=[0,1,2,3,4,5,6,7];
infoWin.__getPn__=function (tr,index){
    var pn=attr(tr,'odds');
    if(pn){
        pn=pn.split(',')
    }else{
        return 2.00
    };
    return parseFloat(pn[index])
};
/*
begin
*/
function App() {
    table_vs.__valToChk = [1, 2, 3, 4, 5, 6, 7, 8];
    table_vs.init('table_vs');
    setUserEvent(table_vs, selectTable);

    selectTable.__namePos = [3, 4];
    selectTable.__valToString = { "0": 1, "1": 2, "2": 3, "3": 4, "4": 5, "5": 6, "6": 7, "7": 8 };
    selectTable.init('selectTeams', 'row_tpl')
    selectTable.onHide = function(tr, spf) {
        table_vs.setCheckBox(tr.zid, spf);
    };

    table_vs.onSelect = function(tr, chk) {
        chk.checked ? selectTable.add(tr, chk) : selectTable.del(tr, chk)
        chk.parentNode.className = chk.checked ? selectedColor : 'sp_bg';
    };

    ggAdmin.init('ggList');
    ggAdmin.setSecrecyLevel('ggSecrecyLevel');
    selectTable.onChange = function(data/*Array[int,int(1-3)]*/) {
        var n = data.length;
        if (n != ggAdmin.vsData.length)//如果队列数量有变,切换过关方式
        {
            ggAdmin.setGGType(selectTable.getDanCount() == 0 ? 0 : selectTable.getDanCount(), this.getShowCount() > 0 ? this.getShowCount() : n);
            selectTable.UnsureDanMa(selectTable.getDanCount());
        }
        ggAdmin.update(data);
    };

    ggAdmin.onChange = function() {//合计
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
            ggAdmin.curType = ggAdmin.curType.substring(0, ggAdmin.curType.length - 1);
        }
        ggAdmin.wager = wagerCount;
        ggAdmin.show(wagerCount);
    };

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

        if (Share < 0) { msg("输入的份数非法。"); OK = false; }
        else if (Share == 1) { OK = true; }
        else {
            if (Share > 1) {
                var multiple = parseInt($id('buybs').value);
                var SumNum = parseInt($id('showCount').innerHTML);
                var SumMoney = multiple * 2 * SumNum;
                var ShareMoney = SumMoney / Share;
                var ShareMoney2 = Math.round(ShareMoney * 100) / 100;

                if (ShareMoney == ShareMoney2) OK = true;
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
                o_tb_AssureShare.value = AutoAssureShare;
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

    if ($id('dpUnsureDan') == null) return;
    $id('dpUnsureDan').onchange = function() {
        ggAdmin.onChange();
        ggAdmin.setGGType($id('dpUnsureDan').value, selectTable.getShowCount());
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