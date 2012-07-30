function find(id, owner) {
	if (owner !== undefined) {
		owner = find(owner);
		return owner == null ? [] : owner.getElementsByTagName(id)
	};
	return typeof id == 'string' ? document.getElementById(id) : id
};
function getNext(el){
    var n=el.nextSibling;
    return n&&(n.nodeType!=1||n.tagName.toLowerCase().indexOf('br')!=-1)?getNext(n):n;
};
function each(arr, fn,a,b) {
        var l=arr.length,a=parseInt(a)||0,b=parseInt(b)||l;
        if(a<0)a=Math.max(0,l+a);
        if(b<0)b=Math.max(0,l+b);
        for (var i=a,j=0; i < b; ++i,++j)
            if (false === fn.call(arr[i], j, arr)) break;
        return b-a
};
function attr(o, key,val) {
    if(arguments.length==3){
        o.setAttribute(key,val);
        return o
    };
    var val=o.getAttribute(key);
	return val?val.toString():''//bug: Maxthon reutrn object
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
function on(x,type,fn){
    var args=Array.prototype.slice.call(arguments,3);
    fn.binder=function(){fn.apply(x,[window.event].concat(args))}
    x.attachEvent?x.attachEvent('on'+type,fn.binder):
    x.addEventListener(type,fn.binder,false);
};
function fixEvent(e){
    e = e || arguments.callee.caller.arguments[0]||event;
    if(!e.stopPropagation){
        e.target=e.srcElement;
        e.stopPropagation=function(){e.cancelBubble = true};
        e.preventDefault=function(){e.returnValue = false}
    };
    return e
};
function New(x,box) {
    var k,d=document,el;
    if(x.tag=='#text'){
        el=d.createTextNode(x.value)
    }else{
        el=d.createElement(x.tag||'div');
        for(k in x)if(!/style|tag|sub|text/i.test(k))el[k]=x[k];
        for(k in x['style'])el.style[k]=x['style'][k];
        if(x.sub)for(k=0;k<x.sub.length;++k)
            arguments.callee(x.sub[k],el)
    };
    return box?box.appendChild(el):el
};
function replace(x, o) {
    for (var w in o) x = x.replace(new RegExp('\\{\\$' + w + '\\}', 'gi'), o[w]);
    return x;
};
var IE = !!window.ActiveXObject;
var IE6 = IE && !window.XMLHttpRequest;
var __XHR = (function(ie) {
	if (!ie) return function(){return new XMLHttpRequest};
	var n = 'Microsoft.XMLHTTP';
	try {
		new ActiveXObject(n)
	} catch(e) {
		n = "Msxml2.XMLHTTP"
	}
	return function() {
		return new ActiveXObject(n)
	}
})( !! window.ActiveXObject);
function http(url, opts) {
	var chk = 0, o = __XHR(),no=Function(),
	ini = {
		type: 'GET',
		search: null,
		load: no,
		err: no,
        updated:true
	};
    if(typeof opts =='function')opts={load:opts};//for(url,fn)
    for (var key in opts) ini[key] = opts[key];
	o.open(ini.type, url, true);
	if (ini.updated) chk = http[url] || 0;//get prev modifiedTime
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

function openPostWindow(URL, PARAMS, name) {
    var tempForm = document.createElement("form");
    tempForm.id = "tempForm";
    tempForm.method = "post";
    tempForm.action = URL;
    tempForm.target = name;
    tempForm.style.display = "none";
    //可传入多个参数
    for (var x in PARAMS) {
        var opt = document.createElement("textarea");
        opt.name = x;
        opt.value = PARAMS[x];
        tempForm.appendChild(opt);
    }
    document.body.appendChild(tempForm);
    if (!isIE()) { tempForm.submit(); }
    else {tempForm.fireEvent("onsubmit");}
    tempForm.submit();
    document.body.removeChild(tempForm);
};

function getClientSize() {
	var d=document,de=d.documentElement,db=d.body
	o=!/BackCompat/i.test(document.compatMode)?de:db;
	return {width:o.clientWidth,height:o.clientHeight,x:o.scrollLeft,y:o.scrollTop}
};
Number.prototype.toFixedFix= function(s){
    return parseInt(this * Math.pow( 10, s ) + 0.5)/ Math.pow( 10, s );
};
function $Tran(){//连乘
	var a=Array.prototype.slice.call(arguments),t=1,n=10000;
	if(!a.length)return 0;
	while(a.length)t*=(a.pop()*n)/n;
	return t
};
function intInputChange(input,fn) {
  if(!input)return;
  input.onkeyup = function() {
      if (/\D/.test(this.value)) {
          if (this.value == "0") { this.value = 0; }
          else { this.value = parseInt(this.value) || 1; }
      }
      //fn.call(this)
  };
  input.onblur=function () {
    var v = this.value;
    if (this.value == "0") { this.value = 0; }
    else { this.value = parseInt(this.value) || 1; }
    fn.call(this)
  };
};

function range(a,b,c){
    var d=[],c=c==undefined?1:isNaN(c)?1:c;
    if(b===undefined){
        if(a>0) for(var i=0;i<a;i++)d[i]=i
    }else{
        if(a<b&&c>0)for(;a<b;a+=c)d.push(a);
        if(a>b&&c<0)for(;a>b;a+=c)d.push(a)
    };
    return d
};
function drag(title,body){
    var d=document,w=window,win=body||title;
    title.style.cursor='move';
    title.onmousedown=function (e){
        e=e||event;
        var x=e.clientX,y=e.clientY,fL=win.offsetLeft,fT=win.offsetTop;
        d.onmousemove=function (e){
            e=e||event;
            var tL=fL+(e.clientX-x),tT=fT+(e.clientY-y);
            win.style.left=tL+'px';
            win.style.top=tT+'px';
            w.getSelection?w.getSelection().removeAllRanges():d.selection.empty();
        };
        d.onmouseup=function (){d.onmousemove=null}
    }
};
cookie = function(key, val, opts) {
    if (!arguments[0])return alert(document.cookie.replace(/;/g, '\n'));
    if (val == undefined)return cookie.each(function(id, _val) {if (id == key) return _val})
    return cookie.add(key, val, opts)
};
cookie.each = function(fn) {
  var s = document.cookie;
  if (s == '' || !fn) return;
  var ss = s.split(';');
  for (var i = 0; i < ss.length; ++i) {
    var a = ss[i].split('='),r = fn.call(this, String(a[0]).replace(/\s+|\s+$/g, ''), decodeURIComponent(a[1]), i);
    if (r !== undefined) return r;
  }
};
cookie.add = function(Id, val, opts) {
  opts = opts || {};
  var c = String(Id).replace(/\s+|\s+$/g, '') + "=" + encodeURIComponent(val);
  var live = (new Date()).getTime() + (opts.time || 86400000 /*1000 * 60 * 60 * 24*/);
  if (opts.time) c += ";expires=" + new Date(live).toUTCString();
  if (opts.path) c += ";path=" + opts.path;
  if (opts.domain) c += "; domain="+ opts.domain;
  if (opts.secure) c += ";secure";
  document.cookie = c;
  return this
};
cookie.del = function(key) {
  this.each(function(id) {
    if (id == key || key === undefined)this.add(id, 0, {day: -1})
  })
};
var diffTime = 0;
function stime(setime,setimeymd)
{
	var Stime = setime + 0;
	diffTime ++;
	var sec = setime-setimeymd +diffTime;

	var sys_D = new Date((Stime) * 1000 );
	var sys_Year = sys_D.getFullYear();
	var sys_Month = sys_D.getMonth() + 1;
	var sys_Day = sys_D.getDate();
	var sys_H=Math.floor(sec/3600)%24;
	var sys_M=Math.floor(sec/60)%60;
	var sys_S=sec%60;
	if(sys_M<10)
		sys_M="0"+sys_M;
	if(sys_S<10)
		sys_S="0"+sys_S;
	document.getElementById("sysTimeDisplay").innerHTML = + sys_Month + "月" + sys_Day + "日 " + sys_H + ":" + sys_M + ":" + sys_S;
}