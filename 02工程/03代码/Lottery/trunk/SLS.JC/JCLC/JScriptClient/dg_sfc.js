selectTable.max=3;
selectTable.mapTr=function (tr,chk){
    return document.getElementById('sel'+tr.zid)
};
selectTable.del=function (tr,chk){
    var mapTr=this.mapTr(tr,chk);
    if(mapTr){
        var chks=mapTr.getElementsByTagName('INPUT');
       this.showCheckBox(chks,chk.value,false);
       if(this.isAllCancel(mapTr))mapTr.hide();
       this._onChange()
    }
};
selectTable.add=function (tr,chk){
  var mapTr=this.mapTr(tr,chk);//映射行
  if(this.getShowCount()==this.max){
    if(!mapTr||mapTr.style.display=='none'){
      each(tr.getElementsByTagName('INPUT'),function (){this.checked=false},1);
      each(tr.nextSibling.getElementsByTagName('INPUT'),function (){this.checked=false});
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
        tmp.spf=parseInt(chk.value);
        tmp.preFix = attr(tr, 'zid');        
        tmp.date = attr(tr, 'pdate');
        tmp.pname =attr(tr,'pname');
        tmp.pn=parseFloat(attr(tr,["","win","lost"][tmp.spf]))||0;//赔率
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
selectTable.showCheckBox = function(chks, value, checked) {//显示胜平负
var index = [0, 7, 1, 8, 2, 9, 3, 10, 4, 11, 5, 12, 6][value], o = chks[index];
    o.checked = checked;
    o.parentNode.style.display = checked ? '' : 'none';
};
selectTable.isAllCancel=function (tr){
    var test=true;
    each(tr.getElementsByTagName('INPUT'),function(){if(this.parentNode.style.display!='none')return test=false},1,0);
    return test
};
selectTable.hideAll=function (){//全部关闭
    each($id('tr',this.box),function (){
        if(this.style.display!='none'){
            this.getElementsByTagName('INPUT')[0].onclick()
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
    return data;
};
selectTable.showMoneyInfo=function(){};
selectTable._onChange=function(){
   var NS=this;
   clearTimeout(NS.timer);
   NS.timer=setTimeout(function (){NS.onChange(NS.getData())},10);//连续变化只保留最后一次有效
};
/*

*/
/*
begin
*/
table_vs.__getChks__=function (tr){
    var zid=attr(tr, 'zid');
    if(!zid)return;//not main line
    var header=$id('input',tr)[0];
    var pnTable=$id('pltr_'+zid);
    if(!pnTable)return {head:header,opts:[]};
    tr.pnTr=pnTable;
    var opts=[],all=[];
    each([tr, pnTable], function (){
        var chks=$id('INPUT',this);
        var len=chks.length;
        if(len<1)return false;
        var body=[];
        if(this.getAttribute('c') != null){
          each(chks, function (i){
              body.push(this)
          },0,-1);
        }else{
          each(chks,function (i){
              body.push(this)
          },1,-1);
        }
        all=all.concat(body);
        if (chks.length <= 1) { //已截止的比赛
			opts = null;
		} else {
			opts.push( {
				options:body,
				allSelect:chks[len-1]
			})
		}
    });
    return {
        head:header,
        opts:opts,
        options:all
    }
};
table_vs.__setRow__=function (tr){
    var chks=this.__getChks__(tr);
    if(!chks)return;

    var NS=this;
    this.list.push(tr);
    tr.hide=function (){//行隐藏
        this.style.display='none';
        if(this.pnTr){
            this.pnTr.style.display='none';
            if($id('img_'+tr.zid))$id('img_'+tr.zid).src = '/images/abtn_pl22.gif'
        };
        NS.onHide(this,this.__hideCount++)
    };
    tr.show=function (){//行显示
        tr.style.display='';
		if(this.pnTr){
            this.pnTr.style.display='';
            if($id('img_'+tr.zid))$id('img_'+tr.zid).src = '/images/abtn_pl2.gif'
        };
		NS.onHide(this,this.__hideCount--)
    };
    chks.head.onclick=function (){//隐藏[sender:chk,influence:tr]
        this.checked=true;
        tr.hide()
    };

    if(chks.opts == null || chks.opts.length==0)return;// 过期比赛

    tr.options=chks.opts;
    tr.zid=attr(tr, 'zid');
    tr.id='vs'+tr.zid;

    tr.updatePn=function (data){//更新折率
        NS.__updatePn__(this,data)
    };

    each(chks.opts,function (i,item){//三行
        var opts=this.options;//单行选项
        var allOpt=this.allSelect;
        for(var i=opts.length;i--;){//单选
            opts[i].checked = false; //初始不选中
			opts[i].onclick=function (e){
               allOpt.checked=NS.isAllSelect(opts);
               NS.onSelect(tr,this)
               e = e || window.event;
               if(e.preventDefault)e.stopPropagation();
            };
            opts[i].cancel=function (){//手动取消选择
                this.checked=false;
                allOpt.checked=NS.isAllSelect(opts)
            }
            opts[i].parentNode.onclick = AutoClick;
        };
		allOpt.checked = false; //初始不选中
        allOpt.parentNode.onclick = AutoClick;
        allOpt.onclick=function (e){//全选(末项)
          for(var i=opts.length;i--;){
            if( opts[i].checked!=this.checked){
              opts[i].checked=this.checked;
              NS.onSelect(tr,opts[i])
            }
          }
          e = e || window.event;
          if(e.preventDefault)e.stopPropagation();//Firefox下事件冒泡
        }
    })

};
table_vs.setCheckBox=function (zid,spf){//外部操作checkbox
    var tr=$id('pltr_'+zid);
    if(tr){
        var chks=tr.getElementsByTagName('INPUT');
        each($id('input',tr),function (){//取消客胜
            if(parseInt(this.value, 10) == parseInt(spf, 10)){
                this.cancel();
                this.parentNode.className='sp_bg';
                return false
            }
        })
        each($id('input', tr.previousSibling),function (){//取消主胜
            if(parseInt(this.value, 10) == parseInt(spf, 10)){
                this.cancel();
                this.parentNode.className='sp_bg';
                return false
            }
        })
    }
};
table_vs.showAll = function () {//显示所有
	var list=this.list,el;
	for(var i=list.length;i--;){
		list[i].show();
	}
	this.onHide()
}

function App(){
  table_vs.__valToChk = [0,1,2,3,4,5,6,0,0,0,0,0,1,2,3,4,5,6];

  table_vs.init('table_vs');
  setUserEvent(table_vs,selectTable);
  selectTable.init('selectTeams','row_tpl')

  selectTable.onHide=function (tr,spf){
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

  ggAdmin._onChange = ggAdmin.onChange=function (data){//合计
     data = data || eval(this.vsData.join('+')) || 0;
     this.data = data;
     var bs = +$id('buybs').value;
     $id('zhushu').value = $id('showCount').innerHTML = data;
     $id('beishu').value = $id('showBS').innerHTML = bs;
     $id('showMoney').innerHTML = '￥'+ ($id('totalmoney').value = (data*bs*2).toFixed(2));
     $id('selectMatchNum').innerHTML = this.vsData.length;
     // 调用客户端程序方法
     try {
         window.external.htmlToClient((data * bs * 2).toFixed(2), $id('zhushu').value);
     } catch (err) {
     }
  };

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