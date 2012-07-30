/*
*    易讯日期输入控件
*    author:[ldh]
*    init()初始化;
*    bind()绑定一个文本框;
*    splitChar属性,设置输出日期的分隔符;
*    setInfo() 设置页脚文本;
*    (调用范例)ESONCalendar.init().bind("k1").bind("k2").bind("k3").bind("k4").splitChar="-";
*/
ESONCalendar={
    dateBox:[],splitChar:"-",validDate:new Date(),validPrevDays:0,validNextDays:0,
    ini:{minY:1999,maxY:new Date().getFullYear()},
    init:function(){
	if(!this.inited){
		this.inited=true;
		return this.addStyle().addUI(new Date()).hide().dateUp(new Date(),true);
	}else{return this};
    },
    uanv_tool_CE:function(type,id,parent,className,HTML){
        var obj=document.createElement(type.toUpperCase());
        id&&(obj.id=id);
        className&&(obj.className=className);
        HTML&&(obj.innerHTML=HTML);
        parent||(parent=document.body);
        return parent.appendChild(obj);
    },
    uanv_tool_getWeek:function(date,i){
        var tmp=new Date(date);
        tmp.setDate(i);
        return tmp.getDay();
    },
    uanv_tool_isIn:function(o,parent){
        while(o!=parent&&o!=document.body){o=o.parentNode};
        return o!=document.body;
    },
	uanv_tool_isValidDate:function(date){
		var prevDate=new Date(this.validDate.getFullYear(),this.validDate.getMonth(),this.validDate.getDate()-this.validPrevDays);
		var nextDate=new Date(this.validDate.getFullYear(),this.validDate.getMonth(),this.validDate.getDate()+this.validNextDays);
		status=(prevDate.toLocaleDateString()+"/"+nextDate.toLocaleDateString())
		status=date<prevDate;
		if(this.validPrevDays!=0&&(date<prevDate))return false;
		if(this.validNextDays!=0&&(date>nextDate))return false;
		return true;
	},
    onselect:function(d){this.hotInput&&(this.hotInput.value=d.y+this.splitChar+d.m+this.splitChar+d.d),this.hide()},
    addStyle:function(){
        var cssText="#ESONCalendar_Win{border:1px #3D8ADB solid;background:#A4B9D7;position:absolute;z-index:9999}";
        cssText+="#ESONCalendar_caption{padding:5px;background:#3D8ADB;text-align:center;}";
        cssText+=".esonclear{clear:both}";
        cssText+="#selMonth{margin-right:5px;width:80px}";
        cssText+="#ESONCalendar_table{background:#F6F6F6 url(http://www.500wan.com/info/about/images/index11_02.jpg) 100% 0;border-collapse:collapse;border:1px solid #C0D0E8}";
        cssText+="#ESONCalendar_table th{border:1px #C0D0E8 solid}";
        cssText+="#ESONCalendar_week{background:#EBF5FE}";
        cssText+="#ESONCalendar_week th{font-size:12px;width:32px;height:24px}";
        cssText+="#dateBox{font:normal 12px /120% 'arial';}";
        cssText+="#dateBox th{font-weight:normal}";
        cssText+="#dateBox .unselected{cursor:pointer;background:#F6F6F6;}";
        cssText+="#dateBox .sunday{cursor:pointer;background:#F6F6F6;color:red}";
        cssText+="#dateBox .current,#dateBox .selected{cursor:pointer;background:#FF9900;color:#fff}";
        cssText+="#ESONCalendar_foot{line-height:30px;text-align:center;font-size:12px;color:#003366;background:#EBF5FE}";
        cssText+="#ESONCalendar_Win iframe{position:absolute;z-index:-1;top:0;left:0}";
        var STYLE=document.createElement('style');
        STYLE.setAttribute("type","text/css");
        STYLE.styleSheet&&(STYLE.styleSheet.cssText=cssText)||STYLE.appendChild(document.createTextNode(cssText));
        document.getElementsByTagName('head')[0].appendChild(STYLE);
        return this;
    },
    addUI:function(date){
        this.y=date.getFullYear();this.m=date.getMonth()+1;this.d=date.getDate();
        this.Win=this.uanv_tool_CE("DIV","ESONCalendar_Win");
        //KillSelectIframe=this.uanv_tool_CE("IFRAME",false,this.Win);
        var _caption=this.uanv_tool_CE("DIV","ESONCalendar_caption",this.Win);
        var selYear=this.uanv_tool_CE("SELECT","selYear",_caption);
        var selMonth=this.uanv_tool_CE("SELECT","selMonth",_caption);

        selMonth.onchange=selYear.onchange=function(){ESONCalendar.dateUp(new Date(selYear.value,selMonth.value,ESONCalendar.d))};
        for(var i=0;i<12;i++){
           // var tmp=new Option("一,二,三,四,五,六,七,八,九,十,十一,十二".split(",")[i]+"月",i);
            var tmp=new Option((i+1)+"月",i);
            selMonth.options.add(tmp);
            if(i==this.m-1){tmp.selected="selected"};
        };
        for(var i=this.ini.minY;i<=this.ini.maxY;i++){
            var tmp=new Option(i+'年',i);
            selYear.options.add(tmp);
            if(i==this.y){tmp.selected="selected"};
        };
        this.uanv_tool_CE("DIV",false,_caption,"esonclear");
         var table=this.uanv_tool_CE("TABLE","ESONCalendar_table",this.Win);
        var tbody=this.uanv_tool_CE("TBODY",false,table);
        var tr=this.uanv_tool_CE("TR","ESONCalendar_week",tbody);
        for(var i=0;i<7;i++)    var th=this.uanv_tool_CE("TH",false,tr,false,new String("日一二三四五六").charAt(i));
        tbody=this.uanv_tool_CE("TBODY","dateBox",table);
        for(var i=0;i<6;i++){
            tr=this.uanv_tool_CE("TR",false,tbody);
            for(var j=0;j<7;j++)this.dateBox.push(this.uanv_tool_CE("TH",false,tr,false,"&nbsp;"));
        };
        this.foot=this.uanv_tool_CE("DIV","ESONCalendar_foot",this.Win,false,this.footText);
        this.foot.innerHTML="<strong>竞彩足球</strong> "+this.y+"年"+this.m+"月"+this.d+"日";
        //KillSelectIframe.frameBorder=0;
        //KillSelectIframe.width=this.Win.offsetWidth;
       // KillSelectIframe.height=this.Win.offsetHeight;
       document.onclick=document.body.onclick=function(e){
            e||(e=window.event);
            var src=e.target||e.srcElement;
            var tmp=src.nodeName.toUpperCase();
            if(tmp=="HTML"||tmp=="BODY")return ESONCalendar.hide();
            if(src==ESONCalendar.hotInput||ESONCalendar.uanv_tool_isIn(src,ESONCalendar.Win))return;
            ESONCalendar.hide();
        };
        return this;
    },
    dateUp:function(date,first){
        var space=this.uanv_tool_getWeek(date,1);
        var m2d=31,index=1;
        this.y=date.getFullYear(),this.m=date.getMonth()+1,this.d=date.getDate();
        var isRN=(this.y%4==0&&this.y%4!=100||this.y%100==0&&this.y%400==0);
        if(/-4|-6|-9|-11/.test("-"+this.m)){m2d=30};
        if(this.m==2){m2d=isRN?29:28};
        for(var i=0;i<42;i++){
            var _this=this.dateBox[i];
            _this.isSunday=_this.className=_this.isInMonth=_this.onmouseover=_this.onmouseout=_this.onclick=null;
            if(i<space||i>(m2d+space-1)){_this.uanv_tool_isInMonth=false;_this.innerHTML="&nbsp;";continue};
            _this.innerHTML=index++;
            if(!this.uanv_tool_isValidDate(new Date(this.y,this.m-1,_this.innerHTML))){
				_this.title="不能选择限定区域外的日期";
				_this.style.cursor="not-allowed";
				continue;
			};
			_this.title="";
			_this.style.cursor="pointer";
            _this.className="unselected";
            _this.isInMonth=true;
            var week=this.uanv_tool_getWeek(date,_this.innerHTML);
            if(week==0||week==6){
                _this.className="sunday";
                _this.isSunday=true;
            }

            if(first&&(index-1)==this.d)_this.className="selected";
            _this.onmouseover=function(){if(this.className!="selected")this.className="current"};
            _this.onmouseout=function(){if(this.className!="selected")this.className=this.isSunday?"sunday":"unselected"};
			_this.onclick=function(){
                var allD=ESONCalendar.dateBox;
                for(var i=0;i<allD.length;i++){
                    var _for=allD[i];
                    _for.className="";
                    if(_for.isInMonth)_for.className="unselected";
                    if(_for.isSunday)_for.className="sunday";
                };
                this.className="selected";
                ESONCalendar.d=this.innerHTML;
                ESONCalendar.onselect({y:ESONCalendar.y,m:ESONCalendar.m,d:this.innerHTML});
				ESONCalendar.hide();
            };
        };
        return this;
    },
    showTo:function(obj){
        var oldObj=obj;
        for(var pos={x:0,y:0};obj;obj=obj.offsetParent){pos.x+=obj.offsetLeft;pos.y+=obj.offsetTop};
        this.Win.style.left=pos.x+"px";this.Win.style.top=(pos.y+2+oldObj.offsetHeight)+"px";
        this.Win.style.display="";return this;
    },
    bind:function(input){
        "string"==typeof(input)&&(input=document.getElementById(input));
        if(input.tagName.toLowerCase()=='input'){
        	input.onfocus=function(){ESONCalendar.showTo(ESONCalendar.hotInput=this)};
        }else{
        	input.onclick=function(e){
        		fixEvent(e).stopPropagation();
        		ESONCalendar.showTo(ESONCalendar.hotInput=this)
        	};
        }
        return this;
    },
    hide:function(){this.Win.style.display="none";return this},
    setInfo:function(v){this.foot.innerHTML=v;return this},
	valid:function(date,PrevDays,NextDays){
		this.validDate=date||new Date();
		this.validPrevDays=PrevDays||0;
		this.validNextDays=NextDays||0;
		return this;
	}
};