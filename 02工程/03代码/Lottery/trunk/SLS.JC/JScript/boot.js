//FrameWork基本信息
var fw = {};
fw.author = "编程浪子";
fw.version = "2.1 Beta";
fw.cdate = "2008-05-01";
fw.iswindow = /window/i.test(navigator.userAgent);
fw.isie = /msie/i.test(navigator.userAgent);
fw.isff = /firefox/i.test(navigator.userAgent);
fw.isop = /opera/i.test(navigator.userAgent);
fw.browser = {
	name : fw.isie ? "IE" : fw.isff ? "Firefox" : fw.isop ? "Opear" : "other",
	version : (function (Ag){
        var ver=Ag.match(/(?:msie|firefox|opera|Chrome|Safari)[ |\/]([\d\.]+)/i);
        return ver?ver[1]:''
    })(navigator.userAgent)
};
fw.isie6 = fw.isie && fw.browser.version=="6.0";

//基础类
fw.ajax = {};
fw.array = {};
fw.array2 = {};
fw.code = {};
fw.conv = {};
fw.cookie = {};
fw.date = {};
fw.dom = {};
fw.event = {};
fw.image = {};
fw.json = {};
fw.lang = {};
fw.math = {};
fw.re = {};
fw.oa = {};
fw.object = {};
fw.string = {};
fw.ui = {};
fw.util = {};
fw.xml = {};
fw.com = {};
fw.com.ui = {};

//IE与FF的兼容代码
if (fw.isie) {
	//event对象
	fw.event.getEvent = function (){
		var e = window.event;
		if (!e.target){
			e.target = e.srcElement;
		}
		return e;
	}
	//xmlHttp对象
	fw.ajax.xmlHttp = function (){
		if(typeof this._http != 'undefined')return new ActiveXObject(this._http);
		var a = ["Msxml2.XMLHTTP.3.0","Msxml2.XMLHTTP","Microsoft.XMLHTTP","Msxml2.XMLHTTP.4.0","Msxml2.XMLHTTP.5.0"];
		for (var i=0,l=a.length;i<l;i++){
			try{
				return (new ActiveXObject((this._http = a[i])));
			}catch(e){};
		}
		return false;
	}
	//xmlDom对象
	fw.xml.xmlDom = function (){
		if(typeof this._Doc != 'undefined')return new ActiveXObject(this._Doc);
		var a = ["MSXML2.DOMDocument", "Microsoft.XMLDOM", "MSXML.DOMDocument", "MSXML3.DOMDocument"];
		for (var i=0,l=a.length;i<l;i++){
			try{
				return new ActiveXObject((this._Doc=a[i]));
			}catch(e){};
		}
		return false;
	}
	//字符转换为XML文档
	fw.conv.str2xmlDoc = function (str){
		var xmlDom = fw.xml.xmlDom();
		xmlDom.async = false;
		xmlDom.loadXML(str);
		return xmlDom.documentElement;
	}
	fw.dom.getAll = function (v){
		return document.all(v);
	}
	fw.dom.getOuterHTML = function (obj){
		return obj.outerHTML;
	}
	fw.dom.setOuterHTML = function (obj,html){
		obj.outerHTML = html;
	}
	fw.dom.getInnerText = function (obj){
		return obj.innerText;
	}
	fw.dom.setInnerText = function(obj,text){
		obj.innerText = text;
	}
	fw.dom.addEvent = function(obj,evt,handle){
		obj.attachEvent("on"+evt,handle);
	}
	fw.dom.fireEvent = function (obj,evt){
		obj.fireEvent("on"+evt);
	}
	fw.dom.setOpacity = function(obj,num){
		obj.style.filter += "alpha(opacity="+(num*100)+")";
	}
	fw.xml.getXml = function(node){
		return node.xml;
	}
	fw.xml.getText = function (node){
		return node.text;
	}
	fw.xml.selectSingleNode = function (node,xPath){
		return node.selectSingleNode(xPath);
	}
	fw.xml.selectNodes = function (node,sExpr){
		return node.selectNodes(sExpr);
	}
	fw.event.capture = {
		start : function (obj){
			obj.setCapture();
		},
		end : function (obj){
			obj.releaseCapture();
		}
	}
}else{
	fw.event.getEvent = function (){
		var e,f=arguments.callee;
		while (f=f.caller) {
			if ((e=f.arguments[0])&&/Event/.test(e.constructor.toString())) {
				return e;
			}
		}
	}
	fw.ajax.xmlHttp = function (){
		return new XMLHttpRequest();
	}
	fw.xml.xmlDom = function (){
		return document.implementation.createDocument("","doc",null);
	}
	fw.conv.str2xmlDoc = function (str){
		return new DOMParser().parseFromString(str,"application/xml").documentElement;
	}
	fw.dom.getAll = function (v){
		return typeof(v)=="string" ? (fw.getId(v)?fw.getId(v):(fw.getName(v)?fw.getName(v)[0]:false)) : v;
	}
	fw.dom.getOuterHTML = function (obj){
		return document.createElement("div").appendChild(obj.cloneNode(true)).parentNode.innerHTML;
	}
	fw.dom.setOuterHTML = function (obj,html){
		var tmpObj = document.createElement("div");
		tmpObj.innerHTML = html;
		obj.parentNode.replaceChild(tmpObj,obj);
	}
	fw.dom.getInnerText = function (obj){
		return obj.textContent;
	}
	fw.dom.setInnerText = function(obj,text){
		obj.textContent = text;
	}
	fw.dom.addEvent = function(obj,evt,handle){
		obj.addEventListener(evt,handle,false);
	}
	fw.dom.fireEvent = function (obj,evt){
		var e = document.createEvent('MouseEvent');
		e.initEvent(evt,false,false);
		obj.dispatchEvent(e);
	}
	fw.dom.setOpacity = function(obj,num){
		obj.style.opacity = num;
	}
	fw.xml.getXml = function(node){
		return new XMLSerializer().serializeToString(node);
	}
	fw.xml.getText = function(node){
		return node.textContent;
	}
	fw.xml.selectSingleNode = function (node,xPath){
		var r = node.nodeType == 9;
		var doc = r ? node : node.ownerDocument;
		var nsRes = doc.createNSResolver(r ? node.documentElement : node);
		var xpRes = doc.evaluate(xPath, node, nsRes, 9, null);
		return xpRes.singleNodeValue;
	}
	fw.xml.selectNodes = function (node,sExpr){
		var r = node.nodeType == 9;
		var doc = r ? node : node.ownerDocument;
		var nsRes = doc.createNSResolver(r ? node.documentElement : node);
		var xpRes = doc.evaluate(sExpr, node, nsRes, 5, null);
		var res = [];
		var item;
		while (item = xpRes.iterateNext())	{
			res[res.length] = item;
		}
		return res;
	}
	fw.event.capture = {
		start : function(obj){
			window.captureEvents(Event.MouseMove|Event.MouseUp);
		},
		end : function(obj){
			window.releaseEvents(Event.MouseMove|Event.MouseUp);
		}
	}
}


//载入文件
fw.ajax.callFile = function (url, callBack, isgb){
	var xmlHTTP = fw.ajax.xmlHttp();
	var async = !!callBack;
	xmlHTTP.open ("get",url,async);
	if (fw.isff){
		xmlHTTP.overrideMimeType("text/html;charset=gb2312");
	}
	if (async){
		xmlHTTP.onreadystatechange = fw.ajax.onStateChange(xmlHTTP,callBack);
		xmlHTTP.send(null);
	}else{
		xmlHTTP.send(null);
		var o = fw.ajax.result(xmlHTTP);
		return o.success ? o.text : false;
	}
}

//AJAX类,默认post异步
fw.ajax.request = function (json){
	var isPost = json.method!="get";
	var arg = json.arg || {};
	json.form && fw.object.merge(arg, fw.dom.getForm(json.form));
	if (isPost){
		arg = fw.url.add("", arg).slice(1);
	}else{
		json.url = fw.url.add(json.url, arg);
	}
	var xmlHTTP = fw.ajax.xmlHttp();
	xmlHTTP.open (json.method||"post", json.url, json.async||true);
	if (isPost)	{
		fw.isff && xmlHTTP.setRequestHeader("Content-Length",arg.length);
		xmlHTTP.setRequestHeader("Content-Type","application/x-www-form-urlencoded"+(json.charset?";charset="+json.charset:""));
	}
	fw.isff && xmlHTTP.overrideMimeType("text/html;charset=gb2312");
	xmlHTTP.onreadystatechange = fw.ajax.onStateChange(xmlHTTP, json.onsuccess, json.onfail);
	xmlHTTP.send(isPost?arg:null);
}

//状态改变时
fw.ajax.onStateChange = function (xmlHTTP, success, fail){
	return function(){
		if (xmlHTTP.readyState!=4) return;
		var o = fw.ajax.result(xmlHTTP);
		if (o.success&&success) success(o.text);
		else if (!o.success&&fail) fail(false);
	}
}

//返回结果时
fw.ajax.result = function(xmlHTTP){
	if (xmlHTTP.status==0||xmlHTTP.status==200){
		var txt = (fw.isie && xmlHTTP.getAllResponseHeaders().toLowerCase().indexOf('charset')==-1) ? fw.lang.bin2ascii(xmlHTTP.responseBody) : xmlHTTP.responseText;
		return { success:true, text:txt };
	}
	return { success:false, text:xmlHTTP.status };
}

//数组相加
fw.array.add = function(a){
	var n = 0;
	for (var i=0,l=a.length;i<l;i++) n+=a[i];
	return n;
}

//克隆数组（包括arguments转换）
fw.array.clone = function (a){
	return fw.array.each(a, function(v){return v}, []);
}

//历遍数组get
fw.array.callEach = function(a, cb){
	for (var i=0,l=a.length;i<l;i++) cb(a[i], i);
}

//产生数字数组
fw.array.createNum = function (n1, n2){
	var a = [];
	for (var i=n1; i<=n2; i++) a.push(i);
	return a;
}

//平分数组
fw.array.divide = function(a, n){
	var r = [];
	while(a.length>0) r.push(a.splice(0,n));
	a = r;
	return a;
}

//历遍数组set
fw.array.each = function(a, cb, r){
	if(r) for(var i=0,t,l=a.length;i<l;i++)(t=cb(a[i],i))!=undefined&&r.push(t);
	else for(var i=a.length-1;i>=0;i--)(a[i]=cb(a[i],i))==undefined&&a.splice(i,1);
	return r||a;
}

//拆分数组
fw.array.splitEach = function(a, cb){
	var r = [];
	for (var i=0,l=a.length;i<l;i++) r=r.concat(cb(a[i]));
	return r;
}

//位数不满自动补零
fw.array.formatNo = function (s,e,n){
	n = n || String(e).length, z = Array(n).join("0"), a = [];
	while(s<=e){
		a.push(z.concat(s++).slice(-n));
	}
	return a;
}

//获取索引号
fw.array.getIdx = function(a, v){
	for (var i=0,l=a.length;i<l&&a[i]!=v;i++);
	return i<l ? i : -1;
}

//获取相同元素的个数
fw.array.getNum = function (a){
	var r = [], o = {};
	for (var i=0,l=a.length; i<l; i++){
		o[a[i]] ? o[a[i]]++ : o[a[i]]=1;
	}
	for (var j in o) r.push([j,o[j]]);
	return r;
}

//判断glob是否包含child中的元素
fw.array.hasMember = function (glob, child){
	var hash = {};
	var i,l;
	for (i=0,l=child.length;i<l;hash[child[i++]]=true);
	for (i=0,l=glob.length;i<l&&!hash[glob[i]];i++);
	return i<l;
}

//判断数组数据是否有重复
fw.array.hasRepeat = function (a){
	var b = {};
	for (var i=0,l=a.length; i<l&&!b[a[i]]; b[a[i++]]=1);
	return i<l;
}

//判断child是否glob子集
fw.array.isChild = function (glob, child){
	var hash = {};
	var i,l;
	for (i=0,l=glob.length;i<l;hash[glob[i++]]=true);
	for (i=0,l=child.length;i<l&&hash[child[i]];i++);
	return i==l;
}

//取最大值
fw.array.max = function (a){
	return Math.max.apply(null,a);
}

//取最小值
fw.array.min = function (a){
	return Math.min.apply(null,a);
}

//数组相乘
fw.array.multiple = function(a){
	var n = 1;
	for (var i=0,l=a.length;i<l;i++) n*=a[i];
	return n;
}

//插入数组
fw.array.insert = function (a, b, i){
	a.splice.apply(a, [i,0].concat(b) );
	return a;
}

//获取数组中的随机数
fw.array.random = function(arr, num){
	var t = fw.array.clone(arr);
	var count = arr.length;
	return fw.math.each(num, function(){
		return t.splice(fw.math.random(0,--count),1);
	});
}

//删除其中一项
fw.array.remove = function (arr,idx){
	var v = arr[idx];
	arr.splice(idx,1);
	return v;
}

//删除其中一项
fw.array.removeText = function (arr, text){
	return fw.array.remove(arr, fw.array.getIdx(arr,text));
}

//数字排序
fw.array.sortNumber = function(a, ad){
	var f = ad!="desc" ? function(a,b){return a-b} : function(a,b){return b-a};
	return a.sort(f);
}

//选择其中几项
fw.array.select = function (){
	var old = arguments[0];
	var a = [];
	for (var i=1,l=arguments.length;i<l;i++) {
		a[i-1] = old[arguments[i]];
	}
	return a;
}

//元素交换
fw.array.swap = function(arr,i,j){
	var temp = arr[i];
	arr[i] = arr[j];
	arr[j] = temp;
}

//字符转换为数字
fw.array.str2num = function (arr_string){
	var arr_number;
	fw.array.callEach(arr_string,function(val){
		arr_number.push( Number(val) );
	});
	return arr_number;
}

//取补集
fw.array.supplement = function (glob,child){
	var hash = {};
	for (var i=0,l=child.length;i<l;i++){
		hash[child[i]] = true;
	}
	var newArr = [];
	for (var i=0,l=glob.length;i<l;i++){
		if (!hash[glob[i]]) {
			newArr.push(glob[i]);
		}
	}
	return newArr;
}

//返回一个不重复的数组
fw.array.uniquity = function(a){
	for (var o={},r=[],i=0,l=a.length;i<l;i++){
		if (!o[a[i]]){
			r.push(a[i]);
			o[a[i]] = true;
		}
	}
	return r;
}

//历遍2维数组
fw.array2.callEach = function(aa, callBack){
	for (var r=0,rows=aa.length;r<rows;r++){
		for(var c=0,cols=aa[r].length;c<cols;c++) callBack(aa[r][c],r,c);
	}
}

//获取2维数组的列数集合
fw.array2.getCols = function(aa){
	var a = [];
	for (var r=0,l=aa.length;r<l;r++){
		a[r] = aa[r].length;
	}
	return a;
}

//数组相加
fw.oa.addBy = function(oa, p){
	var n = 0;
	for (var i=0,l=oa.length;i<l;i++) n+=oa[i][p];
	return n;
}

//获取数组的列数集合
fw.oa.getColsBy = function(oa, p){
	var a = [];
	for (var i=0,l=oa.length; i<l; i++){
		a[i] = oa[i][p].length;
	}
	return a;
}

//获取索引号
fw.oa.getIdxBy = function (oa, p, v){
	for (var i=0,l=oa.length;i<l&&oa[i][p]!=v;i++);
	return i<l ? i : -1;
}

//数组相乘
fw.oa.multipleBy = function(oa, p){
	var n = 1;
	for (var i=0,l=oa.length;i<l;i++) n*=oa[i][p];
	return n;
}

//排序
fw.oa.sortNumberBy = function(oa, p, ad){
	var f = ad!="desc" ? function(a,b){return a[p]-b[p]} : function(a,b){return b[p]-a[p]};
	return oa.sort(f);
}

//显示表单所有的值
fw.code.alertForm = function (Form){
	var a = [];
	var ele = Form.elements;
	for(var i=0;i<ele.length;i++){
		if (ele[i].name) a[a.length] = ele[i].name + " = " + ele[i].value;
	}
	alert(a.join("\n"));
}

//弹出对象的各个属性
fw.code.alertObj = function (obj){
	var a = [];
	for (var i in obj)	{
		a.push(i+" = "+obj[i]);
	}
	alert(a.join("\n"));
}

//获取函数的注释文本
fw.code.getFunText = function (f){
	if (fw.isff){
		return false;
	}
	return fw.trim(f.toString().replace(/^function\s*\w*\s*\(\)\{\/\*([\s\S]*)\*\/\}$/,"$1"));
}

//运行代码
fw.code.run = function (html){
	var doc = window.open('','_blank').document;
	doc.write(html);
	doc.close();
}

//保存代码
fw.code.save = function (str, fileName){
	if (fw.isff) return false;
	var obj = fw.getId("iframe_saveCode") || fw.append(document.body,"iframe",{style:{disaplay:"none",id:"iframe_saveCode"}});
	window.setTimeout(function(){
		var d = obj.contentWindow.document;
		d.charset = "gb2312";
		d.body.innerHTML = str;
		d.execCommand("saveAs", false, fileName);
	},1);
	return true;
}

//获取中文日期格式
fw.conv.d2cd = function (){
	return new Date().toLocaleDateString()+" 星期"+"日一二三四五六".charAt( new Date().getDay() );
}

//字节数格式化
fw.conv.formatByte = function (num){
    return num<1<<10 ? num+" B" : (num<1<<20 ? (num/(1<<10)).toFixed(2)+" KB" : (num/(1<<20)).toFixed(2)+" MB");
}

//位数不满自动补零
fw.conv.formatNo = function (m,n){
	return Array(n).join("0").concat(m).slice(-n);
}

//数字格式化
fw.conv.formatNum = function (m,n){
	m = Number(m);
	var s = m.toLocaleString();
    return isNaN(n) ? s : ( n==0 ? m.toFixed(0) : s.split(".")[0]+"."+m.toFixed(n).split(".")[1] );
}

//数字格式化
fw.conv.formatRMB = function (m,n){
	m = Number(m)
	m = m.toLocaleString().split(".")[0]+(n==0?"":"." + m.toFixed(n||2).split(".")[1]);
    return "￥" + m.replace(/^\./, '0.');
}

//中文数字转化为阿拉伯数字
fw.conv.gb2num = function (s){
	return "零一二三四五六七八九十".indexOf(s);
}

//毫秒转换为秒
fw.conv.ms2s = function (num){
	return (num/60000+":"+num/1000%60).replace(/\.\d+/g,"").replace(/(^|:)(\d)(?!\d)/g,"$10$2");
}

//数字转化为繁体中文
fw.conv.num2big = function (i){
	return "零壹贰叁肆伍陆柒捌玖拾".split("")[i];
}

//阿拉伯数字转化为中文数字
fw.conv.num2gb = function (i){
	return "零一二三四五六七八九".split("")[i];
}

//人民币反格式化
fw.conv.rmb2number = function(rmb){
	return Number(rmb.replace(/[^\d\.]/g,""));
}

//秒转换为毫秒
fw.conv.s2ms = function (str){
	var t = str.split(":");
	return t[0] * 60000 + t[1] * 1000;
}

// @name :未知变量转换成数字
// @parameters: s 字符串, d 返回值, x保留几位小数
fw.conv.toNum = fw.toNum = function (s, d, x){
	return parseFloat((s=parseFloat(s),isNaN(s))?(d||0):s.toFixed((x||0)));
}

//把XML所有子节点的值转换为Json的形式
fw.conv.xml2json = function (pNode,blm){
	var nodes = fw.childNodes(pNode);
	var Result = {};
	for (var i=0;i<nodes.length;i++){
		var node = nodes[i];
		name = node.nodeName;
		if ( fw.childNodes(node) ){
			value = node;
		}else{
			value = fw.trim( fw.xml.getText(node) );
			if (!blm){
				if (/^\d+$/.test(value)){
					value = Number(value);
				}else if(/^true$/i.test(value)){
					value = true;
				}else if(/^false$/i.test(value)){
					value = false;
				}
			}
		}
		Result[name] = value;
	}
	return Result;
}

//setCookie by xqin
fw.cookie.set = function(n, v, t){
    var exp  = new Date();
    exp.setTime(exp.getTime() + (t||24)*60*60*1000);
    document.cookie = n + "="+ escape(v) + ";expires=" + exp.toGMTString()+';path=/';
}

//getCookie by xqin
fw.cookie.get = function(n){
	var arr = document.cookie.match(new RegExp("(^| )"+ n +"=([^;]*)(;|$)"));
	if(arr != null) return unescape(arr[2]);
	return null;
}

//格式化时间
fw.date.format = function(dateStr){
	return new Date(dateStr.replace(/[\-\u4e00-\u9fa5]/g, "/"));
}

//返回某年某月的天数(即最后一天的日期)
fw.date.getLastDate = function (y,m){
	return new Date(y, m, 0).getDate();
}

//定时
fw.date.timer = function (){
	var t = 0;
	this.reset = function(){
		t = new Date();
	}
	this.getTime = function(){
		return new Date() - t;
	}
	this.reset();
}

//author by 刘党辉
//用法: alert(new Date().show('yy-M-d hh:mm:ss:S 星期w'))
fw.date.show = function(x, dt){
	var f0=function(x,n){return ('0000'+x).slice(-(n||2))}
	with(dt||new Date())
		var $={	'yyyy':getFullYear()	,'M':getMonth()+1,'d':getDate()
			,'h':getHours()	,'m':getMinutes()	,'s':getSeconds(),'S':getMilliseconds()
			,'w+':'日一二三四五六'.charAt(getDay())
		};
	$.MM=f0($.M);
	$.dd=f0($.d);
	$.hh=f0($.h);
	$.mm=f0($.m);
	$.ss=f0($.s);
	$.SS=f0($.s,4);
	if(/y{4,}/i.test(x)){x=x.replace(/y{4,}/,$['yyyy'])}
	else{x=x.replace(/y+/,f0($['yyyy']))}
	for (var re in $){x=x.replace(new RegExp('\\b'+re+'\\b'),$[re])}
	return x;
}

//两个元素对齐
fw.dom.align = function(o1, o2){
	o2.style.left = o1.style.left;
	o2.style.top = o1.style.top;
	o2.style.width = o1.offsetWidth + "px";
	o2.style.height = o1.offsetHeight + "px";
}

//创建Dom对象
fw.dom.append = function(parentObj,tag,args){
	var obj = fw.dom.create(tag,args);
	parentObj.appendChild(obj);
	return obj;
}

//获取节点集合
fw.dom.childNodes = function (node){
	//if(!node||!node.childNodes)return false;
	var oo = fw.get(node).childNodes;
	var tmp = [];
	for (var i=0,l=oo.length;i<l;i++){
		if (oo[i].nodeType==1){
			tmp.push(oo[i]);
		}
	}
	tmp.firstChild = tmp[0];
	return tmp.length>0 ? tmp : false;
}

//清除子节点
fw.dom.clearChild = function(node){
	node = fw.get(node);
	while(node.firstChild){
		node.removeChild(node.firstChild);
	}
}

//点击对象
fw.dom.click = function (obj){
	if (fw.isie) return obj.click();
	var evt = obj.ownerDocument.createEvent('MouseEvents');
	evt.initMouseEvent('click', true, true, obj.ownerDocument.defaultView, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
	obj.dispatchEvent(evt);
}

//创建Dom对象
fw.dom.create = function(tag,args){
	var obj = document.createElement(tag);
	if (args){
		for (var item in args){
			if (item=="style") {
				fw.dom.setStyle(obj,args[item]);
			}else if (item=="range"){
				obj.style.position = "absolute";
				fw.dom.setRange(obj,args[item]);
			}else if (item=="html"){
				obj.innerHTML = args.html;
			}else if (item=="className"){
				obj.className = args[item];
			}else{
				obj.setAttribute(item,args[item]);
			}
		}
	}
	return obj;
}

//返回ID对象,参数为id或object
fw.dom.get = function (v){
	return typeof(v)=="string" ? fw.getId(v) : v;
}

//获取属性集合
fw.dom.getAttribute = function (attName,attValue){
	return fw.dom.getObjAttribute(document.body,attName,attValue);
}

//获取复选框的值
fw.dom.getCheckbox = function (name){
	var o = fw.dom.getName(name);
	var a = [];
	for(var i=0,l=o.length;i<l;i++){
		o[i].checked && a.push(o[i].value);
	}
	return a;
}

//获取class
fw.dom.getClass = function (name){
	fw.dom.getObjClass(document.body,name);
}

//返回页面上所有的ID对象
fw.dom.getDocumentId = function (){
	for (var oo={},a=fw.dom.getTag("*"),l=a.length,i=0;i<l;i++){
		var o = a[i];
		if (o.id){
			oo[o.id] = o;
		}
	}
	return oo;
}

//获取表单所有的值
fw.dom.getForm = function (form) {
	form = typeof(form)=="string" ? document.forms[form] : form;
    var o = {}, t;
    for (var i=0,l=form.elements.length; i<l; i++) {
    	t = form.elements[i];
        t.name!="" && (o[t.name]=t.type=="radio"?fw.dom.getRadio(t.name):t.value);
    }
    return o;
}

//getHeight
fw.dom.getHeight = function(o){
	return parseInt(o.style.height) || 0;
}

//document.getElementById的简写
fw.dom.getId = function (id){
	return document.getElementById(id);
}

//返回整个页面的所有图片
fw.dom.getImg = function (){
	return document.images;
}

//获取文本输入框的值
fw.dom.getInputNum = function (name){
	var o = fw.dom.getName(name);
	var a = [];
	for(var i=0,l=o.length;i<l;i++) a.push(+o[i].value);
	return a;
}

//getLeft
fw.dom.getLeft = function(o){
	return parseInt(o.style.left) || 0;
}

//document.getElementsByName的简写
fw.dom.getName = function (name){
	return document.getElementsByName(name);
}

//获取属性集合
fw.dom.getObjAttribute = function (obj, attName, attValue) {
    var isMatch = function (attValue) {return typeof attValue == "string" ? attName == attValue : attValue.test(attName);};
    var o = fw.dom.getObjTag(obj, "*");
    var a = [];
    for (var i = 0, l = o.length; i < l; i++) {
        if (isMatch(o[i].getAttribute(attName))) {
            a.push(o[i]);
        }
    }
    return a;
}

//获取objclass
fw.dom.getObjClass = function (obj, value) {
    var o = fw.dom.getObjTag(obj, "*");
    for (var i = 0, l = o.length; i < l; i++) {
        if (o[i].className == value) {
            return o[i];
        }
    }
    return false;
}

//返回某节点下的所有图片
fw.dom.getObjImg = function (obj){
	return fw.get(obj).getElementsByTagName("img");
}

//获取节点下的tag
fw.dom.getObjTag = function (obj,tagName){
	return fw.get(obj).getElementsByTagName(tagName);
}

//获取不同DTD的主体
fw.dom.getOwner = function (){
	return document.compatMode&&document.compatMode!="BackCompat" ? document.documentElement : document.body;
}

//获取窗口尺寸
fw.dom.getOwnerSize = function (){
	var d = fw.dom.getOwner();
	return { width : d.clientWidth, height : d.clientHeight };
}

//获取单选框的值
fw.dom.getRadio = function (name){
	var o = fw.dom.getName(name);
	for(var i=o.length-1;i>-1&&!o[i].checked;i--);
	return i>-1 ? o[i].value : false;
}

//getRange
fw.dom.getRange = function(o){
	return { x:fw.dom.getLeft(o), y:fw.dom.getTop(o), w:fw.dom.getWidth(o), h:fw.dom.getHeight(o) };
}

//getSize
fw.dom.getSize = function(o){
	return { w:fw.dom.getWidth(o), h:fw.dom.getHeight(o) };
}

//获取tag
fw.dom.getTag = function (tagName){
	return document.getElementsByTagName(tagName);
}

//返回array对象集合,参数为id或object
fw.dom.getToArray = function (){
	for (var a=[],l=arguments.length,i=0;i<l;i++){
		a[i] = fw.get(arguments[i]);
	}
	return a;
}

//返回object对象集合,参数为id
fw.dom.getToObject = function (){
	for (var oo={},l=arguments.length,i=0;i<l;i++){
		oo[arguments[i]] = fw.getId(arguments[i]);
	}
	return oo;
}

//getTop
fw.dom.getTop = function(o){
	return parseInt(o.style.top) || 0;
}

//获取图像绝对尺寸
fw.dom.getUtterSize = function (img){
	var h = /Rotation\s*=\s*[02]/i.test(img.style.filter) ? img.width : img.height;
	var v = /Rotation\s*=\s*[13]/i.test(img.style.filter) ? img.width : img.height;
	return { h:h, v:v };
}

//获取元素绝对位置
fw.dom.getUtterXy = function (o){
	for(var _pos={x:0,y:0};o;o=o.offsetParent){
		_pos.x+=o.offsetLeft;
		_pos.y+=o.offsetTop;
	};
	return _pos;
}

//getWidth
fw.dom.getWidth = function(o){
	return parseInt(o.style.width) || 0;
}

//getXy
fw.dom.getXy = function(o){
	return { x:fw.dom.getLeft(o), y:fw.dom.getTop(o) };
}

//获取节点集合
fw.dom.selectChild = function (){
	var args = fw.array.clone(arguments);
	var arr = fw.childNodes(args.shift());
	args.unshift(arr);
	return fw.array.select.apply(null,args);
}

//setHeight
fw.dom.setHeight = function(o,px){
	o.style.height = px + "px";
}

//setHTML
fw.dom.setHTML = function(o, html){
	fw.get(o) && (fw.get(o).innerHTML=html);
}

//setLeft
fw.dom.setLeft = function(o,px){
	o.style.left = px + "px";
}

//setRange
fw.dom.setRange = function(o,r){
	fw.dom.setXy(o,r[0],r[1]);
	fw.dom.setSize(o,r[2],r[3]);
}

//setSize
fw.dom.setSize = function(o,w,h){
	fw.dom.setWidth(o,w);
	fw.dom.setHeight(o,h);
}

//设置样式
fw.dom.setStyle = function(obj,st){
	for (var i in st){
		obj.style[i] = st[i];
	}
}

//setTop
fw.dom.setTop = function(o,px){
	o.style.top = px + "px";
}

//setWidth
fw.dom.setWidth = function(o,px){
	o.style.width = px + "px";
}

//setXy
fw.dom.setXy = function(o,x,y){
	fw.dom.setLeft(o,x);
	fw.dom.setTop(o,y);
}

//insertRows
fw.dom.insertRows = function (tb,html){
    var o = document.createElement("div");
    o.innerHTML = "<table><tbody>"+html+"</tbody></table>";
    var ol = o.childNodes[0].tBodies[0].rows;
    while(ol.length>0){
        tb.tBodies[0].appendChild(ol[0]);
    }
}

//insertRows
fw.dom.insertRows2 = function (tb, html, isclear){
	tb = fw.get(tb);
	isclear && fw.dom.clearChild(tb);
    var o = document.createElement("div");
    o.innerHTML = "<table><tbody>"+html+"</tbody></table>";
    var ol = o.childNodes[0].tBodies[0].rows;
    while(ol.length>0) tb.appendChild(ol[0]);
}

//检查图片位置是否超出指定的范围
fw.image.chkRange = function (img,Range){
	var x1 = Range.x1;
	var x2 = Range.x2;
	var y1 = Range.y1;
	var y2 = Range.y2;
	//var r  = img.filters["DXImageTransform.Microsoft.BasicImage"].Rotation % 2;
	var r  = 0//img.filters[0].Rotation % 2;
	var w  = r ? y2-y1 : x2 - x1;
	var h  = r ? x2-x1 : y2 - y1;

	if (img.width>w){
		img.height = Math.round(w/img.width*img.height);
		img.width = w;
	}
	if (img.height>h){
		img.width  = Math.round(h/img.height*img.width);
		img.height = h;
	}

	var size = fw.dom.getUtterSize(img);

	if (fw.dom.getLeft(img)<x1)	{
		fw.dom.setLeft(img,x1);
	}else if(fw.dom.getLeft(img)>x2-size.h){
		fw.dom.setLeft(img,x2-size.h);
	}

	if (fw.dom.getTop(img)<y1){
		fw.dom.setTop(img,y1);
	}else if (fw.dom.getTop(img)>y2-size.v){
		fw.dom.setTop(img,y2-size.v);
	}
}

//预载图片回调函数
fw.image.callImage = function (url,callBack){
	callBack = callBack || new Function();
	url = url.split(",");
	for (var i=0,l=url.length;i<l;i++){
		var o = new Image();
		o.onload = function(){
			callBack(o);
			o.onload = null;
		}
		o.onerror = function(){
			callBack(false);
		}
		o.src = url[i];
	}
}

//获取比例宽高
fw.image.getScale = function (w1,h1,w2,h2){
	if (w1>w2){
		h1 = Math.round(w2/w1*h1);
		w1 = w2;
	}
	if (h1>h2){
		w1 = Math.round(h2/h1*w1);
		h1 = h2;
	}
	return {w:w1,h:h1};
}

//图片预加载类
fw.image.preLoad = function (){
	var me = this;
	var a = [];		//图片数组
	this.idx = 0;	//已经被加载的图片个数
	this.num = 0;	//图片总数

	//下载图片
	this.loadImages = function(s){
		a = s.split(",");
		this.num = a.length;
		downImg();
	}

	function downImg() {
		var img = new Image();
		var isLoad = false;
		img.onload = function(){
			if (!isLoad){
				isLoad = true;
				me.Idx++;
				if (me.loadIng){
					me.loadIng();
				}
				if (me.idx<me.num){
					downImg();
				}else if (me.callBack){
					me.callBack(a);
				}
			}
		}
		img.src = a[me.idx];
	}
}

//按照一定的比例显示图片
fw.image.showScale = function (img,w,h){
	if (img.width>w){
		img.height = Math.round(w/img.width*img.height);
		img.width = w;
	}
	if (img.height>h){
		img.width = Math.round(h/img.height*img.width);
		img.height = h;
	}
}

//按照一定的比例缩放图片
fw.image.zoomScale = function (img,w,h){
	img.height = Math.round(w/img.width*img.height);
	img.width = w;
	if (img.height>h){
		img.width = Math.round(h/img.height*img.width);
		img.height = h;
	}
}

//转义字符替换函数
fw.json._replaceFunc = function(character) {
	var a = {
    	'\b': '\\b',
    	'\t': '\\t',
    	'\n': '\\n',
    	'\f': '\\f',
    	'\r': '\\r',
    	'"' : '\\"',
		'\\': '\\\\'
	};
	if (a[character]) {
		return a[character];
	}
	var n = character.charCodeAt();
	return '\\u00' + Math.floor(n/16).toString(16) + (n%16).toString(16);
}

//数组转换为JSON字符串
fw.json.array2string = function (arr) {
	var a = [];
	for (var i=0,l=arr.length; i<l; i++) {
		a[i] = fw.json.tostring(arr[i]);
	}
	return "["+a.join(",")+"]";
}

//布尔值转换为JSON字符串
fw.json.boolean2string = function (bln) {
	return String(bln);
}

//日期转换为JSON字符串
fw.json.date2string = function (dt) {
	return fw.date.format(dt, '"yyyy-mm-dd hh:mm:ss"');
}

//数字转换为JSON字符串
fw.json.number2string = function (num) {
	return isFinite(num) ? String(num) : "null";
}

//JS对象转换为JSON字符串
fw.json.object2string = function (obj) {
	var a = [];
	for (var k in obj) {
		if (obj.hasOwnProperty(k)) {
			//a.push( fw.json.tostring(k) + ":" + fw.json.tostring(obj[k]) );
			a.push( k + ":" + fw.json.tostring(obj[k]) );
		}
	}
	return "{" + a.join(",") + "}";
}

//解析JSON对象
fw.json.parse = function(jsonString) {
	var result = false;
	try {
		result = eval('(' + jsonString + ')');
	}catch (e) {};
	return result;
}

//字符对象转换为JSON字符串
fw.json.tostring2string = function (str) {
    if (/["\\\x00-\x1f]/.test(str)) {
		str = str.replace(/[\x00-\x1f\\"]/g, fw.json._replaceFunc);
    }
    return '"' + str + '"';
}

//转换为JSON字符串
fw.json.tostring = function (variant) {
	switch (typeof variant) {
		case 'object':
			if (variant) {
				if (typeof variant.string === 'function'){
					return (variant.string());
				}
				var clz = variant.constructor;
				if (clz == Array){
					return fw.json.array2string(variant);
				}else if (clz == Date){
					return fw.json.date2string(variant);
				}else if (clz == Object){
					return fw.json.object2string(variant);
				}
			} else {
				return 'null';
			}
		case 'string':
			return fw.json.tostring2string(variant);
		case 'number':
			return fw.json.number2string(variant);
		case 'boolean':
			return fw.json.boolean2string(variant);
		case "function":
			return variant.toString();
	}
}

//二进制转为ASCII码
fw.lang.bin2ascii = function (bstr){
	var s = false;/*
	try{
		var rec = new ActiveXObject("ADODB.RecordSet");
		rec.Fields.Append("DDD",201,1);
		rec.open();
		rec.addNew();
		rec(0).appendChunk(bstr);
		rec.update();
		s = rec(0).value;
		rec.close();
		rec = null;
	}catch(e){*/
		window.bstr = bstr;
		window.execScript('Dim I,S,tc,nc:S="":For I=1 To LenB(bstr):tc=AscB(MidB(bstr,I,1)):If tc<&H80 Then:S=S&Chr(tc):Else:nc=AscB(MidB(bstr,I+1,1)):S=S&Chr(CLng(tc)*&H100+CInt(nc)):I=I+1:End If:Next', "vbscript");
		s = window.S;
	//}
	return s;
}

//绑定
fw.lang.bind = function() {
	var args = fw.array.clone(arguments);
	var obj = args.shift();
	var method = args.shift();
	//method.prototype;
	// assert method != null;
	if (typeof method != "function"){
		throw "Invalid method: " + method;
	}
	return function() {
		var iargs = [];
		for (var i = 0; i < arguments.length; i++){
			iargs.push(arguments[i]);
		}
		return method.apply(obj, args.concat(iargs));
	}
}

//冒泡排序
fw.math.bubbleSort = function(arr){
	for (var i=arr.length-1;i>0;i--){
		for (var j=0;j<i;j++){
			if (arr[j]>arr[j+1]){
				fw.array.swap(arr,j,j+1);
			}
		}
	}
}

//循环
fw.math.each = function(l,cb){
	var r = [];
	for (var i=0; i<l; i++) r[i]=cb(i);
	return r;
}

//2D循环
fw.math.each2 = function(rows,cols,callBack){
	for (var r=0;r<rows;r++){
		for(var c=0;c<cols;c++){
			callBack(r,c);
		}
	}
}

//计算阶乘
fw.math.F = function (n){
    var c = n;
    while (n>1) c*=--n;
    return c;
}

//排列个数
fw.math.P = function (n,m){
	var c = 1;
	for (var i=n-m; i<n; c*=++i);
	return c;
}

//组合个数
fw.math.C = function (n,m){
	var n1=1, n2=1;
	for (var i=n,j=1; j<=m; n1*=i--,n2*=j++);
	return n1/n2;
}

//获取指定范围的随机数
fw.math.random = function (s,e){
	return Math.floor(Math.random()*(e+1-s)) + s;
}

//大数相乘
fw.math.factorialLarge = function (n){
	var a = [1];
	for (var i=1;i<=n;++i){
		for (var j=0,c=0;j<a.length||c!=0;++j){
			var m = j<a.length ? (i*a[j]+ c) : c;
			a[j] = m % 10;
			c = (m-a[j]) / 10;
		}
	}
	return a.reverse().join("");
}

//获取指定范围的随机数
fw.math.randomNums = function (startNum,endNum,count,repeat){
	var ret = [];
	if (repeat){
		for (var i=0;i<count;i++){
			ret[i] = fw.math.random(startNum,endNum);
		}
		return ret;
	}
	var tmp = [];
	var i = 0;
	for (var s=startNum;s<=endNum;s++){
		tmp[i++] = s;
	}
	var l = tmp.length;
	for (i=0;i<count;i++){
		ret[i] = fw.array.remove(tmp,fw.math.random(0,--l));
	}
	return ret;
}

//组合不闭包
fw.math.combine = function (a,n,t){
	if (n==0) return [t];
	var r = [];
	for(var i=0; i<=a.length-n; i++){
		r = r.concat(arguments.callee(a.slice(i+1), n-1, t.concat(a[i])));
	}
	return r;
}

//组合结果
fw.math.CR = function (arr, num){
	var r = [];
	(function f(t,a,n){
		if(n==0) return r.push(t);
		for(var i=0,l=a.length-n; i<=l; i++){
			f(t.concat(a[i]), a.slice(i+1), n-1);
		}
	})([],arr,num);
	return r;
}

//替换组合
fw.math.RC = function (arr, num, rule, tag){
	var r = [];
	(function f(a, n, s){
		if (n==0) return r.push(a);
		for (var i=s; i>=0; i--){
			rule(i) && f(a.slice(0,i).concat([tag]).concat(a.slice(i+1)), n-1, i-1);
		}
	})(arr, num, arr.length-1);
	return r;
}

//排列结果
fw.math.permute = fw.math.PR = function (arr, num){
    var r = [];
    (function f(t,a,n){
        if (n==0) return r.push(t);
        for (var i=0,l=a.length; i<l; i++){
            f(t.concat(a[i]), a.slice(0,i).concat(a.slice(i+1)), n-1);
        }
    })([],arr,num);
    return r;
}

//定位排列
fw.math.PL = function (a){
	var r = [];
	var n = a.length;
	(function f(t,i){
		if(i==n) return r.push(t);
		for(var j=0,l=a[i].length; j<l; j++){
			f(t.concat(a[i][j]), i+1);
		}
	})([],0);
	return r;
}

//克隆对象
fw.object.clone = function (o){
	var r = {};
	for(var i in o){
		r[i] = o[i];
	}
	return r;
}

//深度克隆对象
fw.object.cloneAll = function(o){
	var r;
	if (o.constructor==Object) {
		r = {};
		for(var i in o){
			r[i] = arguments.callee(o[i]);
		}
	}else if (o.constructor==Array){
		r = [];
		for(var i=0;i<o.length;i++){
			r[i] = arguments.callee(o[i]);
		}
	}else{
		return o;
	}
	return r;
}

//历遍调用
fw.object.callEach = function(o, cb){
	for (var i in o) cb(o[i], i);
}

//历遍修改
fw.object.each = function(o, cb, a){
	if (a){ for (var i in o) a.push(cb(o[i],i)); return a };
	for (var i in o) o[i]=cb(o[i],i);
}

//合并对象
fw.object.merge = function (o1, o2) {
    for (var i in o2) {
        o1[i] = o2[i];
    }
    return o1;
}

//email地址
fw.re.email = function (condition) {
    return fw.re.parse(/\w+(?:[\-\.]\w+)*@\w+(?:[\-\.]\w+)*\.(?:com|cn|net|org|cc|tv|hk|biz|info|com\.cn|net\.cn|org\.cn|gov\.cn)/, condition);
}

//中文
fw.re.gb = function (condition) {
    return fw.re.parse(/[\u4e00-\u9fa5]+/, condition);
}

//是否email
fw.re.isEmail = function (str) {
    return fw.re.email("bi").test(str);
}

//是否中文
fw.re.isGb = function (str) {
    return fw.re.gb("b").test(str);
}

//是否手机号码
fw.re.isMobile = function (str) {
    return fw.re.mobile("b").test(str);
}

//是否数字
fw.re.isNumber = function (str) {
    return fw.re.number("b").test(str);
}

//是否用户名
fw.re.isName = function (str) {
    return fw.re.name("b").test(str);
}

//是否固定电话
fw.re.isPhone = function (str) {
    return fw.re.phone("b").test(str);
}

//是否邮编
fw.re.isPost = function (str) {
    return fw.re.post("b").test(str);
}

//是否QQ
fw.re.isQq = function (str) {
    return fw.re.qq("b").test(str);
}

//是否电话
fw.re.isTel = function (str) {
    return fw.re.tel("b").test(str);
}

//是否账户
fw.re.isUid = function (str) {
    return fw.re.uid("b").test(str);
}

//是否URL
fw.re.isUrl = function (str) {
    return fw.re.url("bi").test(str);
}

//手机号码
fw.re.mobile = function (condition) {
    return fw.re.parse(/1(?:3\d|5\d|8\d)-?\d{8}/, condition);
}

//用户名，只能输入中文、字母、数字、下划线、横杠
fw.re.name = function (condition) {
    return fw.re.parse(/[\u4e00-\u9fa5\w\-]+/, condition);
}

//数字
fw.re.number = function (condition) {
    return fw.re.parse(/\-?\d+(?:\.\d+)?/, condition);
}

//正则解释
fw.re.parse = function (re,condition){
	re = condition.indexOf("b")>-1 ? "^"+re.source+"$" : re.source;
	return new RegExp(re, condition.replace("b",""));
}

//固话小灵通号码(含分机号)
fw.re.phone = function (condition) {
    return fw.re.parse(/\d{3,4}-?\d{7,8}(?:\-\d{1,4})?/, condition);
}

//邮编
fw.re.post = function (condition) {
    return fw.re.parse(/\d{6}/, condition);
}

//QQ
fw.re.qq = function (condition) {
	return fw.re.parse(/[1-9][0-9]{4,10}/, condition);
}

//电话号码
fw.re.tel = function (condition) {
    return fw.re.parse(/1(?:3\d|5\d)-?\d{8}|\d{3,4}-?\d{7,8}(?:\-\d{1,4})?/, condition);
}

//账号，只能输入字母、数字、下划线，且必须以字母或下划线开头
fw.re.uid = function (condition) {
    return fw.re.parse(/[a-z_]\w*/, condition);
}

//网址
fw.re.url = function (condition) {
    return fw.re.parse(/http:\/\/\w+(?:[\-\.]\w+)*\.(?:com|cn|net|org|cc|tv|hk|biz|info|com\.cn|net\.cn|org\.cn|gov\.cn)(?:[\-\.\/]\w+)*\/?/, condition);
}

//获取字符真实长度
fw.string.len = function (s){
	return s.replace(/[^\x00-\xff]/g,"aa").length;
}

//字符串反转
fw.string.reverse = function(s){
    return s.split("").reverse().join("");
}

//删除字符串前后多余的空格
fw.string.trim = function (s,m){
	return s.replace(new RegExp("^[\\s\\u3000]+|[\\s\\u3000]+$",m?"mg":"g"),"");
}

//获取URL参数
fw.string.urlget = function (url,p){
	var o = fw.string.urlparse(url);
	return o[p];
}

//分析URL参数
fw.string.urlparse = function (url){
	var o = {};
	url = url.split("?");
	if (url.length>1){
		var a = url[1].split("&");
		for (var i=0;i<a.length;i++){
			var s = a[i].split("=");
			o[s[0]] = s[1];
		}
		return o;
	}
	return {};
}

//设置URL参数
fw.string.urlset =function (url,p,v){
	var o = fw.string.urlparse(url);
	o[p] = v;
	var a = [];
	for (var i in o){
		a.push(i+"="+o[i]);
	}
	return url.replace(/\?.+$/,"") + "?" + a.join("&");
}

//判断是否为空
//@author: huangjp
fw.empty = function (p)
{
	if (typeof(p) == "undefined" || typeof(p) == "unknown" || p == null) return true;

	if (p.length <= 0) return true;

	return false;
}

fw.url = {};

//批量获取URL的值
fw.url.get = function(url){
	var json = {};
	url.replace(/[\?\&](\w+)=(\w+)/g,function(s,s1,s2){
		json[s1] = s2;
	});
	return json;
}

//批量设置URL的值
fw.url.add = function(u, o){
	var a = [];
	for (var i in o){
		a.push(i+"="+o[i]);
	}
	return u + "?" + a.join("&");
}

//设置URL的值
fw.url.set = function(){
	var a = fw.array.clone(arguments);
	var o = a[1];
	var l = a.length;
	var r = [];
	if (l>2){
		for (var i=2; i<l; i++) r.push(a[i]+"="+o[a[i]]);
	}else{
		for (var i in o) r.push(i+"="+o[i]);
	}
	return a[0] + "?" + r.join("&");
}

//绑定子节点列表事件
fw.ui.addListEvent = function (parent,callBack,e){
	var o = fw.childNodes( fw.get(parent) );
	for(var i=0,l=o.length;i<l;i++) {
		o[i]["on"+(e||"click")] = (function(n){return function(){callBack(n)};})(i);
	}
}

//创建动态文本框
fw.ui.createFileBox = function(obj,filebox){
	if (!filebox) {
		filebox = fw.append(document.body,"span",{html:'<input type="file" />'});
		fw.dom.setOpacity(filebox,0);
	}
	var r = fw.dom.getRange(obj);
	fw.dom.setRange(filebox,[r.x,r.y,r.w,r.h]);
	filebox.scrollLeft = fw.childNodes(filebox)[0].offsetWidth - r.w;
}

//拖动
fw.ui.drag = function (obj, obj2){
	obj = fw.get(obj);
	if (!obj) return;
	obj2 = fw.get(obj2) || obj;
	with (obj.style) position!="absolute"&&(position="absolute");
	with (obj2.style) cursor!="move"&&(cursor="move");
	var xy, evt;
	obj2.onmousedown = function (){
		fw.event.capture.start(obj);
		evt = fw.event.getEvent();
		xy = fw.dom.getXy(obj);
		xy = {
			x : evt.clientX  - xy.x,
			y :	evt.clientY  - xy.y
		}
		document.onmousemove = over;
		document.onmouseup = up;
	}
	function over(){
		evt = fw.event.getEvent();
		fw.dom.setXy(obj, evt.clientX-xy.x, evt.clientY-xy.y);
	}
	function up(){
		fw.event.capture.end(obj);
		document.onmousemove = null;
		document.onmouseup = null;
	}
}

//Flash生成类
fw.ui.flash = function (u,w,h,param){
	param = param || {};
	param.quality = "high";
    if (fw.isie){
    	param.movie = u;
    	var f = function(v, name){ return '<param name="'+name+'" value="'+v+'">' };
    	var a = fw.object.each(param, f, []);
		var id = "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000";
		var cb = "http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0";
		return '<object classid="'+id+'" codebase="'+cb+'" width="'+w+'" height="'+h+'">'+a.join('')+'</object>';
    }else{
    	param.src = u;
    	var f = function(v, name){return ' '+name+'="'+v+'"' };
    	var a = fw.object.each(param, f, []);
		return '<embed type="application/x-shockwave-flash"'+a.join('')+' width="'+w+'" height="'+h+'"></embed>';
    }
}

//Iframe高度自适应
fw.ui.iframeAutoFit = function (){
    var a = fw.dom.getTag("iframe");
    for (var i=0;i<a.length;i++){
        try{
            var d = a[i].contentWindow.document;
            var h = Math.min(d.documentElement.scrollHeight,d.body.scrollHeight);
            fw.dom.setHeight(a[i],h);
        }catch(e){};
    }
}

//创建播放器
fw.ui.player = function(url,w,h){
	var html = '<object data="'+url+'" width="'+w+'" height="'+h+'" '+
						'mediawrapchecked="true" type="video/quicktime" loop="true" autoplay="true" align="bottom" border="0">'+
			   '</object>';
	return html;
}

//清空文件选择框并保留事件
fw.ui.resetFile = function (obj){
	var name = obj.name;
	var form = obj.form;
	var ChangeEvent = obj.onchange;
	fw.dom.setOuterHTML(obj,fw.dom.getOuterHTML(obj));
	form[name].onchange = ChangeEvent;
}

//对象定位
fw.ui.setPos = function (o,x,y){
	o = fw.get(o);
	var root = fw.dom.getOwner();
	var w = root.clientWidth;
	var h = root.clientHeight;
	var w2 = o.offsetWidth;
	var h2 = o.offsetHeight;
	switch (x){
		case "l": x = 0; break;
		case "r": x = w - w2; break;
		case "c": x = Math.round(w/2-w2/2); break;
	}
	switch (y){
		case "t": y = 0; break;
		case "b": y = h - h2; break;
		case "m": y = Math.round(h/2-h2/2) + fw.dom.getOwner().scrollTop;
	}
	fw.dom.setXy(o,x,y);
}

//把对象定位在网页正中间
fw.ui.setCenter = function (o,blm){
	o = fw.get(o);
	if (o.style.position!="absolute"){
		o.style.position = "absolute";
	}
	fw.ui.setPos(o,"c","m");
	if (blm){
		var d = fw.dom.getOwner();
		fw.dom.setTop(o,Math.round(d.clientHeight/2-o.offsetHeight/2+d.scrollTop));
	}
}

//tab切换
fw.ui.tb = function (ol, css1, css2, cb, evt){
	if (ol.constructor!=Array) ol = fw.childNodes(ol);
	var idx = 0;
	var handle = function (i){
		if (idx==i) return;
		ol[idx].className = css2;
		ol[i].className = css1;
		cb && cb(i,idx);
		idx = i;
	}
	var f = function(i){ return function(){handle(i)} };
	evt = "on" + (evt||"click");
	for (var i=0,l=ol.length; i<l; i++){
		if (css1!=""&&ol[i].className==css1) idx = i;
		ol[i][evt] = f(i);
	}
	this!=window && (this.idx = idx, this.chg = handle);
}

//上传回调
fw.ui.uploadFile = function (form,b){
	form = typeof(form)=="string" ? document.forms[form] : form;
	var target = "uploadFileWindow";
	var ifr = fw.dom.getName(target);
	if (ifr&&ifr.length>0){
		ifr = ifr[0];
	}else{
		ifr = fw.append(document.body,"iframe",{style:{display:"none"}});
		//ifr = fw.append(document.body,"iframe",{style:{position:"absolute",left:"0px",top:"0px",width:"600px",height:"300px"}});
		ifr.contentWindow.name = target;
	}
	form.target = target;
	if (b) form.submit();
	return true;
}

//窗口类
fw.ui.window = function(oo){
	var win = fw.append(document.body,"div",{style:{
		position:"absolute",backgroundColor:"#ffffff",display:"none",padding:"10px"
	}});
	fw.dom.setRange(win,[100,100,oo.size[0],oo.size[1]]);
	var sh = new fw.ui.showHide(win);
	this.show = function(){
		sh.stepShow();
	};
	this.hide = function(){
		sh.stepHide();
	};
	var tmp = fw.append(win,"div");
	fw.append(tmp,"span",{html:oo.title});
	fw.append(tmp,"span",{html:"X"}).onclick = fw.bind(this,this.hide);
	this.panel = fw.append(win,"div");
	this.setHTML = function(html){
		this.panel.innerHTML = html;
	}
}


//最大化窗口
fw.ui.windowMax = function (){
	window.moveTo(0,0);
	window.resizeTo(window.screen.availWidth,window.screen.availHeight);
}

//数字变红
fw.util.chgNumRed = function (v){
	return String(v).replace(/[\d\.]+/g,"$&".fontcolor("#ff0000"));
}

fw.util.chkDate = function (str){
	if (!/^(\d{4})\-(\d{1,2})\-(\d{1,2})$/.test(str)) return false;
	var y = +RegExp.$1, m = +RegExp.$2, d = +RegExp.$3;
	var n = m==2?(y%4||!(y%100)&&y%400?28:29):(/4|6|9|11/.test(m)?30:31);
	return m<=12 && d<=n;
}

//去掉以空格相隔的字符串中的任何一个关键字
fw.util.clearKeyWord = function (str,w){
	return str.replace(new RegExp("^"+w+"\\s","i"),"").replace(new RegExp("\\s"+w+"(\\s)","i"),"$1").replace(new RegExp("\\s"+w+"$","i"),"");
}

//返回日期差距
fw.util.diffDate = function (d1,d2){
	d1 = typeof(d1)=="string" ? new Date(d1.replace(/\-/g,"/")) : d1;
	d2 = typeof(d2)=="string" ? new Date(d2.replace(/\-/g,"/")) : d2;
	if (d1>d2) d1=[d2,d2=d1][0];

	//相差年数
	this.diffYears = function(){
		return d2.getFullYear() - d1.getFullYear();
	}

	//相差天数
	this.diffDays = function(){
		return Math.floor((d2.getTime()-d1.getTime())/86400000);
	}

	//相差详细时间
	this.diffTime = function(){
		var o = this.diffData();
		return o.year+"年"+o.month+"个月"+o.day+"天"+o.hour+"小时"+o.minute+"分"+o.second+"秒";
	}

	//相差的数据
	this.diffData = function(){
		var years = 0;
		var y = d1.getFullYear();
		while(d1<=d2){
			d1.setFullYear(++years+y);
		}
		d1.setFullYear(--years+y);
		var months = 0;
		var m = d1.getMonth();
		while(d1<=d2){
			d1.setFullYear(years+y,++months+m);
		}
		d1.setFullYear(years+y,--months+m);

		var t = d2.getTime() - d1.getTime();
		var d = Math.floor(t/86400000);
		t -= 86400000 * d;
		var h = Math.floor(t/3600000);
		t -= 3600000 * h;
		var m = Math.floor(t/60000);
		t -= 60000 * m;
		var s = Math.floor(t/1000);
		return { year:years, month:months, day:d, hour:h, minute:m, second:s};
	}
}

//定时
fw.util.timer = function (){
	var t = 0;
	this.reset = function(){
		t = new Date();
	}
	this.getTime = function(){
		return new Date() - t;
	}
	this.reset();
}

//获取远程XML
fw.xml.callRemoteXml = function (url,callBack){
	return fw.callXml(fw.actFile+"?url="+url,callBack);
}

fw.xml.XMLCache={};
//载入XML文件
fw.xml.callXml = function(url, callBack, isDom){
	var xmlDom = fw.ajax.xmlHttp(), async = (typeof callBack != 'undefined');
	if(url.indexOf('.xml?')!=-1)url = url.split('.xml?')[0]+'.xml';
	xmlDom.open('GET', url, async);
	xmlDom.setRequestHeader('If-Modified-Since', (fw.xml.XMLCache[url]?fw.xml.XMLCache[url].lastModified:'0'));
	var f = function(xmlDom){return isDom ? xmlDom : xmlDom.documentElement};
	var check = function(xmlDom){
		if(xmlDom.status==200 || xmlDom.status==0){
			var lastModi = xmlDom.getResponseHeader('Last-Modified');
			if(lastModi != null){
				fw.xml.XMLCache[url] = {
					lastModified: lastModi,
					data: xmlDom.responseXML
				};
			}
			return async?callBack(f(xmlDom.responseXML)):f(xmlDom.responseXML);
		}else if(xmlDom.status==304){
			return async?callBack(f(fw.xml.XMLCache[url].data)):f(fw.xml.XMLCache[url].data);
		}
		return null;
	}
	if(async){
		xmlDom.onreadystatechange = function(){
			if(xmlDom.readyState != 4)return;
			return check(xmlDom);
		}
	}
	xmlDom.send(null);
	if(!async)return check(xmlDom);
}

//childNodes
fw.xml.childNodes = fw.dom.childNodes;

//批量获取attribute的值
fw.xml.getAtt = function (){
	var json = {}, a = arguments, node = a[0], l = a.length;
	if (l>1){
		for (var i=1;i<l;i++) json[a[i]] = node.getAttribute(a[i]) || false;
	}else{
		for (var i=0,a=node.attributes,l=a.length;i<l;i++) json[a[i].nodeName] = a[i].nodeValue;
	}
	return json;
}

//简写
fw.append = fw.dom.append;
fw.bind = fw.lang.bind;
fw.each = fw.array.each;
fw.callEach = fw.array.callEach;
fw.callFile = fw.ajax.callFile;
fw.callImage = fw.image.callImage;
fw.callXml = fw.xml.callXml;
fw.childNodes  = fw.dom.childNodes;
fw.create = fw.dom.create;
fw.get = fw.dom.get;
fw.getId = fw.dom.getId;
fw.insert = fw.dom.insert;
fw.setHTML = fw.dom.setHTML;
fw.trim = fw.string.trim;

/*---------------加载机制部分-----------------*/

//onready事件
fw.onReady = function (callBack){
	if (fw.isff) return window.addEventListener("load",callBack,false);
	var f = function(){ /loaded|complete/i.test(document.readyState) && callBack() };
	document.attachEvent("onreadystatechange", f);
}

fw.file = {};
fw.file.item = {};
fw.file.obj = {}
fw.head = fw.dom.getTag("head")[0];
fw.tmp = fw.dom.getTag("script")[fw.dom.getTag("script").length-1];
fw.path = fw.tmp.src.match(/(.*)boot\.js/)[1];
if (fw.isie && !(/^((?:http:\/)?\/.*)boot\.js/i.test(fw.tmp.src))){
	fw.path = document.URL.replace(/[^\/\\]+$/,"") + fw.path;
	while(/([\/\\]\w+[\/\\]\.\.)/.test(fw.path)){
		fw.path = fw.path.replace(RegExp.$1,"");
	}
}
fw.arg = fw.url.get(fw.tmp.src);

fw.img = function(img){
	return fw.path + "images/" + img;
}
fw.tmp = null;
var SmsId ={
	'1,1':'012',//胜负彩复式合买
	'1,2':'012',//复式代购
	'1,3':'012',//单式合买
	'1,4':'012',//单式代购
	'1,43':'013',//任九复式
	'1,44':'013',//任九单式
	'1,123':'014',//任九胆拖
	'3,1':'032',//双色球复式合买
	'3,3':'032',//单式合买
	'3,135':'033',//胆拖合买
	'4,1':'041',//七星彩复式合买
	'4,3':'041',//单式合买
	'5,20':'051',//数字排列排列3直选复式
	'5,24':'051',//排列3直选单式
	'5,28':'052',//排列3组选3复式
	'5,29':'053',//排列3组选6复式
	'5,30':'054',//直选和值
	'5,31':'055',//组选和值
	'5,39':'055',//组选单式合买
	'5,136':'056',//直选组合复式
	'5,137':'056',//直选组合胆拖复式
	'5,138':'056',//直选跨度复式
	'5,139':'056',//组选3跨度复式
	'5,140':'056',//组选6跨度复式
	'5,141':'056',//组选3胆拖复式
	'5,142':'056',//组选6胆拖复式
	'5,23':'057',//排列5复式
	'5,63':'057',//排列五单式
	'7,1':'072',//福彩3D复式合买
	'7,3':'072',//单式合买
	'7,26':'076',//直选和值
	'7,45':'074',//组选三
	'7,46':'075',//组选六
	'7,47':'075',//组选单式
	'7,48':'0756',//组选六和值
	'7,50':'0746',//组选三和值
	'8,1':'081',//22选5复式合买
	'8,3':'081',//单式合买
	'9,34':'091',//足球单场胜平负复式
	'9,40':'092',//总进球数复式
	'9,41':'093',//上下单双复式
	'9,42':'094',//比分复式
	'9,51':'095',//半全场复式
	'9,150':'095',//胜平负单式
	'9,151':'095',//总进球数单式
	'9,152':'095',//上下单双单式
	'9,153':'095',//比分单式
	'9,154':'095',//半全场单式
	'11,1':'112',//七乐彩复式合买
	'11,3':'112',//单式合买
	'12,52':'121',//时时乐直选代购
	'12,53':'122',//前二代购
	'12,54':'122',//后二代购
	'12,55':'122',//前一代购
	'12,56':'122',//后一代购
	'12,57':'1231',//组选三代购
	'12,58':'1233',//组选六代购
	'12,59':'1210',//直选和值代购
	'12,60':'124',//直选包号代购
	'12,61':'1232',//组选三和值代购
	'12,62':'1234',//组选六和值代购
	'12,67':'125',//前二包号
	'12,68':'125',//后二包号代购
	'12,69':'125',//组选三包号代购
	'12,70':'125',//组选六包号代购
	'12,71':'125',//组选单式代购
	'12,102':'125',//直选复式合买
	'12,72':'125',//组选3复式合买
	'12,73':'125',//组选6复式合买
	'12,75':'125',//前二复式合买
	'12,76':'125',//后二复式合买
	'12,77':'125',//前一复式合买
	'12,78':'125',//后一复式合买
	'12,81':'125',//组选3和值合买
	'12,82':'125',//组选6和值合买
	'12,86':'125',//组选单式合买
	'12,103':'1225',//直选和值合买
	'12,119':'1226',//押一位
	'12,120':'1227',//押二位
	'12,121':'1228',//押一位合买
	'12,122':'1229',//押二位合买
	'15,1':'151',//6场半全场复式合买
	'15,3':'151',//单式合买
	'17,1':'171',//4场进球复式合买
	'17,3':'171',//单式合买
	'28,1':'281',//超级大乐透复式合买
	'28,3':'281',//单式合买
	'28,98':'282',//追加复式
	'28,99':'282',//追加单式
	'28,100':'283',//12选2复式
	'28,101':'283',//12选2单式
	'28,135':'284',//胆拖复式
	'28,143':'285',//胆拖追加
	'29,104':'291',//时时彩五星复式
	'29,105':'291',//三星复式
	'29,106':'291',//二星复式
	'29,107':'291',//一星复式
	'29,108':'292',//五星组合
	'29,109':'292',//三星组合
	'29,110':'292',//二星组合
	'29,111':'293',//大小单双
	'29,112':'293',//五星单式
	'29,113':'293',//三星单式
	'29,114':'293',//二星单式
	'29,115':'293',//混投
	'29,116':'2951',//二星组选复式
	'29,117':'2951',//二星组选单式
	'29,118':'2952',//二星组选和值
	'29,124':'294',//五星通选
	'29,144':'2953',//三星直选组合
	'29,145':'2954',//三星组选三
	'29,146':'2955',//三星组选六
	'29,147':'2956',//三星组选包胆
	'29,148':'2957',//三星组选和值
	'29,149':'2957',//三星组选单式
	'43,155':'431',//十一运夺金前一直选
	'43,156':'431',//前二直选
	'43,157':'431',//前三直选
	'43,158':'4390',//前二组选
	'43,159':'4391',//前三组选
	'43,160':'432',//任选二
	'43,161':'433',//任选三
	'43,162':'434',//任选四
	'43,163':'435',//任选五
	'43,164':'436',//任选六
	'43,165':'437',//任选七
	'43,166':'438',//任选八
	'43,167':'438'//混投
};
function getSmsId(lotid,playid){
	return SmsId[(lotid+','+playid)] || '0';
}