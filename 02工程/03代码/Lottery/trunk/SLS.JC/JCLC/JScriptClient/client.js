
function GetClientCodes() {

    var bs = +$id('beishu').value, zs = +$id('zhushu').value;

    //    if (parseInt($id('totalmoney').value, 10) > 2000000) return alert('方案总金额不能超过2000000!\n请修改投注内容后重试.');
    this.disabled = true;
    var res = [];
    var Dan = [];
    each(selectTable.box.rows, function(i) {

        if (this.style.display != 'none') {
            if (this.cells.length > 3 && this.cells[3].getElementsByTagName('input')[0].checked) {
                Dan.push({
                    preFix: this.preFix,
                    date: +new Date(this.date.replace(/-/g, '/')),
                    pname: parseInt(this.pname),
                    code: getCodesNumber(this.cells[2].getElementsByTagName('label'))
                })
            }
            else {
                res.push({
                    preFix: this.preFix,
                    date: +new Date(this.date.replace(/-/g, '/')),
                    pname: parseInt(this.pname),
                    code: getCodesNumber(this.cells[2].getElementsByTagName('label'))
                })
            }
        }
    });

    res.sort(function(a, b) {
        if (a.date == b.date) {
            return a.preFix - b.preFix
        } else {
            return a.date - b.date
        };
    });

    Dan.sort(function(a, b) {
        if (a.date == b.date) {
            return a.preFix - b.preFix
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
    return $id('codes').value;
}



function getCodesNumber(l) {
    var res = [];

    each(l, function() {
        if (this.style.display != 'none') {
            res.push(getC(this.firstChild.value));
        }
    });
    return '(' + res.join(',') + ')';
}

function getC(v) {
    if ($("#lotType").val() == 'sf') return selectTable.__valToString[v];
    if ($("#lotType").val() == 'rfsf') return table_vs.__valToChk[v];
    if ($("#lotType").val() == 'sfc') return selectTable.__valToChkSFC[v];
    if ($("#lotType").val() == 'dx') return selectTable.__valToString[v];
    return +v;
}

function SetReload() {
    selectTable.hideAll();
}

function _refurbish() {
    window.location.reload();
}