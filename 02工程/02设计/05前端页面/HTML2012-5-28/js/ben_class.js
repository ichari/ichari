// JavaScript Document
/**
  * tab函数
  * new
  * ([tab1,....],[内容1,...],打开中tab样式,关闭中tab样式,事件)
*/
function scrollDoor(){}
scrollDoor.prototype = {
sd : function(menus,divs,openClass,closeClass,sEventType){
	var _this = this;
	if(menus.length != divs.length){alert("菜单层数量和内容层数量不一样!");return false;}
	for(var i = 0 ; i < menus.length ; i++){
		_this.$(menus[i]).value = i;
		_this.ad(_this.$(menus[i]),sEventType,function(){
			for(var j = 0 ; j < menus.length ; j++){
				_this.$(menus[j]).className = closeClass;
				_this.$(divs[j]).style.display = "none";
			}
		_this.$(menus[this.value]).className = openClass;
		_this.$(divs[this.value]).style.display = "";})
	}
},
$ : function(oid){
	if(typeof(oid) == "string")
		return document.getElementById(oid);
		return oid;
	}
,ad : function(oTarget,sEventType,funName){
		if(oTarget.addEventListener){
			oTarget.addEventListener(sEventType,funName, false);
		}else if(oTarget.attachEvent){
			oTarget.attachEvent("on"+sEventType,funName);
		}else{
			oTarget["on"+sEventType]=funName;
		}
	}
}