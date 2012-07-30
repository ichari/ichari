selectTable.max=8;
selectTable.mapTr=function (tr,chk){
    return document.getElementById('sel'+tr.zid)
};
selectTable.del=function (tr,chk){
    var mapTr=this.mapTr(tr,chk);
    if(mapTr){
        var chks=mapTr.getElementsByTagName('INPUT');
       this.showCheckBox(chks,chk.value,false);
       if(this.isAllCancel(mapTr))mapTr.hide();
       this._onChange();       
    }
};
selectTable.add=function (tr,chk){
    var mapTr=this.mapTr(tr,chk);//映射行
   if(this.getShowCount()==this.max){
       if(!mapTr||mapTr.style.display=='none'){
            each(tr.getElementsByTagName('INPUT'),function (){this.checked=false},1);
            return alert('单个方案最多只能选择'+this.max+'场比赛进行投注');
       }
    };
    var newTr=this.getNewTr(tr,chk,mapTr);
    this.box.appendChild(newTr);
    this.reSort();
    this._onChange()
};
selectTable.getNewTr=function (tr,chk,node){//添加行
    var tmp=node,NS=this;
    if(!node){
        tmp=this.tpl.cloneNode(true)
        var tds=tmp.getElementsByTagName('TD');
        tmp.id='sel'+tr.zid;
        tmp.zid=attr(tr,'zid');//zid
        tmp.spf=parseInt(chk.value);//310
        tmp.preFix = attr(tr, 'mid');
        tmp.date = attr(tr, 'pdate');
        tmp.pname =attr(tr,'pname');
        tmp.pn = parseFloat(attr(tr, ["", "lost", "win"][tmp.spf])) || 0; //赔率
        tmp.style.display='';
        tds[0].lastChild.innerHTML=this.unHTML(tr.cells[0].innerHTML);
        tds[1].innerHTML=this.unHTML(tr.cells[3].innerHTML+' vs '+tr.cells[4].innerHTML);
        tmp.hide=function (){this.style.display='none'};
        tmp.show=function (){this.style.display=''};
    }else{
        this.show(tr,chk)
    };
    var chks=tmp.getElementsByTagName('INPUT');
    this.showCheckBox(chks,chk.value,true)
    chks[0].onclick=function (){//隐藏行
       each(tmp.getElementsByTagName('INPUT'),function (){
           this.onclick()
       },1);
       this.checked=true;
       tmp.hide();
    };
    each(chks,function (i){//check事件
        this.onclick=function (){
            this.parentNode.style.display='none';
            NS.onHide(tmp,this.value);
            if(NS.isAllCancel(tmp))tmp.hide();
            NS._onChange()//发起onchange事件
        }
    },1);
    return tmp
};
selectTable.showCheckBox=function (chks,value,checked){//显示胜负
    //对阵表中主负的单选框值为2，选号表中却要显示下标为1的单选框，所以要做转换
	var o=chks[value];
    o.checked=checked;
    o.parentNode.style.display=checked?'':'none';
};
selectTable.isAllCancel=function (tr){
    var test=true;
    each(tr.getElementsByTagName('INPUT'),function(){if(this.parentNode.style.display!='none')return test=false},1,0);
    return test
};
selectTable.hideAll=function (){//全部关闭
    each($id('tr',this.box),function (){
        if(this.style.display!='none'){
            this.getElementsByTagName('INPUT')[0].onclick();
        }
    });
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
/*

*/
ggAdmin={
    ggType:[
		'r2c1',
		'r3c1,r3c3,r3c4',
		'r4c1,r4c4,r4c5,r4c6,r4c11',
		'r5c1,r5c5,r5c6,r5c10,r5c16,r5c20,r5c26',
		'r6c1,r6c6,r6c7,r6c15,r6c20,r6c22,r6c35,r6c42,r6c50,r6c57',
		'r7c1,r7c7,r7c8,r7c21,r7c35,r7c120',
		'r8c1,r8c8,r8c9,r8c28,r8c56,r8c70,r8c247'
	],
    ggValue:eval('('+$id('jsonggtype').value+')'),
    curType:false,
    vsData:[],

    init:function (typeListBox){
        var list=$id('input',typeListBox);
        this.typeList=[];
        for(var i=list.length;i--;)
            this.typeList.push(list[i]);
        var NS=this;
        each(this.typeList,function (){
            this.onclick=function (){//切换过关选择
                if(this.value==NS.curType)return;
                NS.curType=this.value;
                NS._onChange()
            }
        })
    },

    setGGType:function (n){//显示可用过关方式
        this.curType=false;
        var useType=n<2?'':this.ggType.slice(n-2,n-1).join(',');
        each(this.typeList, function(){
          var use=useType.indexOf(this.id)+1;
          this.parentNode.style.display=use?'':'none';
          this.checked = use;
          if(use)this.click();
        });
    },

    update:function (data){
       this.vsData=data;
       this._onChange()
    },

    _onChange:function (){
        if(this.vsData.length<2){
           this.data =this.data=0; 
        }else{
           this.data =get_puy_Count(this.vsData,this.curType);
        }
        this.onChange(this.data)
      
    },

    onChange:function (){}
};
/*
begin
*/
function App(){
    table_vs.init('table_vs');
    setUserEvent(table_vs,selectTable);
    selectTable.init('selectTeams','row_tpl')

    selectTable.onHide=function (tr,spf){
        //选号表中主负的单选框值为2，对阵表中却要隐藏下标为1的单选框，所以要做转换
		spf = spf % 2 + 1;
		table_vs.setCheckBox(tr.zid,spf);
    };
    table_vs.onSelect=function (tr,chk){
        chk.checked?selectTable.add(tr,chk):selectTable.del(tr,chk)
        chk.parentNode.className=chk.checked?selectedColor:'sp_bg';
    };

    ggAdmin.init('ggList');
    selectTable.onChange=function (data/*Array[int,int(1-3)]*/){
        var n=data.length;
        if(n!=ggAdmin.vsData.length)//如果队列数量有变,切换过关方式
            ggAdmin.setGGType(n);
        ggAdmin.update(data);
    };

    ggAdmin.onChange = function(data) {//合计
        var bs = $id('buybs').value;
        $id('zhushu').value = $id('showCount').innerHTML = data;
        $id('beishu').value = $id('showBS').innerHTML = bs;
        $id('showMoney').innerHTML = '￥' + ($id('totalmoney').value = (data * bs * 2).toFixed(2));
        $id('MaxPrice').innerHTML = infoWin.getMaxPrice();
        // 调用客户端程序方法
        try {
            window.external.htmlToClient((data * bs * 2).toFixed(2), $id('zhushu').value);
        } catch (err) {
        }
    };

    infoWin.init('lookDetails','popWindow','popWindowTitle','popContent','showMoney');
    infoWin.onShow=function (){
        if(selectTable.getShowCount()<2)return alert('请最少选择两场比赛!');
        if(!ggAdmin.curType)return alert('请选择过关方式!');
        this.ggTypeSet=ggAdmin.curType;
        this.bs=$id('buybs').value;
        this.show(selectTable)
    }

    intInputChange($id('buybs'),function (){//倍数输入框
        if(isNaN(ggAdmin.data))return;
        ggAdmin.onChange(ggAdmin.data)
    });

    if($id('clearAllSelect') == null)return;
    $id('clearAllSelect').onclick=function (){
        selectTable.hideAll()
    };
};
App();