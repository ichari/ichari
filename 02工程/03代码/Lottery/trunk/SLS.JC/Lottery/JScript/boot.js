var fw = {};
fw.math = {};
fw.array = {};
fw.object = {};

//排列个数
fw.math.P = function(n, m) {
    var c = 1;
    for (var i = n - m; i < n; c *= ++i);
    return c;
}

//组合个数
fw.math.C = function(n, m) {
    var n1 = 1, n2 = 1;
    for (var i = n, j = 1; j <= m; n1 *= i--, n2 *= j++);
    return n1 / n2;
}

//获取数组中的随机数
fw.array.random = function(arr, num) {
    var t = fw.array.clone(arr);
    var count = arr.length;
    return fw.math.each(num, function() {
        return t.splice(fw.math.random(0, --count), 1);
    });
}

//循环
fw.math.each = function(l, cb) {
    var r = [];
    for (var i = 0; i < l; i++) r[i] = cb(i);
    return r;
}

//获取索引号
fw.array.getIdx = function(a, v) {
    for (var i = 0, l = a.length; i < l && a[i] != v; i++);
    return i < l ? i : -1;
}

//获取指定范围的随机数
fw.math.random = function(s, e) {
    return Math.floor(Math.random() * (e + 1 - s)) + s;
}

//克隆数组（包括arguments转换）
fw.array.clone = function(a) {
    return fw.array.each(a, function(v) { return v }, []);
}

//历遍数组set
fw.array.each = function(a, cb, r) {
    if (r) for (var i = 0, t, l = a.length; i < l; i++) (t = cb(a[i], i)) != undefined && r.push(t);
    else for (var i = a.length - 1; i >= 0; i--) (a[i] = cb(a[i], i)) == undefined && a.splice(i, 1);
    return r || a;
}

//合并对象
fw.object.merge = function(o1, o2) {
    for (var i in o2) {
        o1[i] = o2[i];
    }
    return o1;
}

//删除其中一项
fw.array.remove = function(arr, idx) {
    var v = arr[idx];
    arr.splice(idx, 1);
    return v;
}

//删除其中一项
fw.array.removeText = function(arr, text) {
    return fw.array.remove(arr, fw.array.getIdx(arr, text));
}

//获取索引号
fw.array.getIdx = function(a, v) {
    for (var i = 0, l = a.length; i < l && a[i] != v; i++);
    return i < l ? i : -1;
}