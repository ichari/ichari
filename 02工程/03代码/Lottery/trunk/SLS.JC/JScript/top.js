fw.onReady( function(){
	/************* 绑定周榜/月榜/赛季榜点击事件 **************/
	var tabmenus = fw.get("menutab2");
    var list=tabmenus.getElementsByTagName('li');
	for (var i=0; i<list.length && i<3; i++)
	{
        list[i].childNodes[0].onclick = function(){
			try {
				for (var n=0; n<list.length && n<3; n++)
				{
					if (list[n] != this.parentNode)
					{
						list[n].className = "li_01";
					}
					else
					{
						this.parentNode.className = "li_02";
						var query = fw.get("query");
                        var qs=query.getElementsByTagName('DIV')
						for (var m=0; m<qs.length; m++)
							qs[m].style.display = "none";
						
						var p3 = 1;
						
						qs[n].style.display = "block";
					
						p3 = n==0 ? fw.get("week").value :
							n==1 ? (fw.get("year").value+"-"+fw.get("month").value+"-1") :
							fw.get("season").value;
						
						var url = fw.string.urlset(fw.get("fra").src, "p2", (n+1).toString());
						if(this.id!="dlt")
						fw.get("fra").src = fw.string.urlset(url, "p3", p3);
					}
				}
			} catch(ex) {
				alert(ex.description);
			}
		};
	}
	/************************ end ************************/
	fw.get("submit1").onclick = function(){
		fw.get("fra").src = fw.string.urlset(fw.get("fra").src, "p3", fw.get("week").value);
	}
	fw.get("submit2").onclick = function(){
		fw.get("fra").src = fw.string.urlset(fw.get("fra").src, "p3", fw.get("year").value+"-"+fw.get("month").value+"-1");
	}
	fw.get("submit3").onclick = function(){
		fw.get("fra").src = fw.string.urlset(fw.get("fra").src, "p3", fw.get("season").value);
	}
});

function onclickmenu(p, object, lotid)
{
	var table = fw.get("menutab1");
	try {
		for (var a=0; a<table.rows.length; a++)
			for (var b=0; b<table.rows[a].cells.length; b++)
				table.rows[a].cells[b].className = table.rows[a].cells[b].className.replace(/td_02/g,"td_01");
	
		object = object.tagName.toLowerCase()=="td" ? object : object.parentNode;
		object.className = object.className.replace(/td_01/g,"td_02");

		if(p==1)
			fw.get("menutab2").parentNode.style.display = "block";
		else
			fw.get("menutab2").parentNode.style.display = "none";

		var list=fw.get("menutab2").getElementsByTagName('li');
		
		fw.get("fra").src = fw.string.urlset(fw.get("fra").src, "p1", p);
		var d = new Date();
		
		if(p==10||p==11||p==12||p==13){
			
			
			var day = d.getDay();
			if(day==0)
			day = 7;
			day--;
			var mil = d.getTime();
			var nd = new Date(mil-day*24*3600*1000);//本周周一
		    var nyear = nd.getFullYear();
		    var nmonth = nd.getMonth()+1;
		    if(nmonth<10){
		    	nmonth = "0"+nmonth;
		    }
		    var ndate = nd.getDate();
		     if(ndate<10){
		    	ndate = "0"+ndate;
		    }
			fw.get("week").value = nyear+"-"+nmonth+"-"+ndate;
			var d71 = new Date(2010,6,1);
			if(mil>=d71.getTime())
			d.setMonth(d.getMonth()-1);
			
			fw.get("year").value = d.getFullYear();
			
			fw.get("month").value = d.getMonth()+1;
			
			list[0].childNodes[0].onclick();
		} else {
			
			var year = d.getFullYear();
			var month = d.getMonth()+1;
			if(month<10){
				month = "0"+month;
			}
			var day = d.getDate();
			if(day<10){
				day = "0"+day;
			}
			
			fw.get("week").value = year+"-"+month+"-"+day;
			fw.get("year").value = year;
			fw.get("month").value = d.getMonth()+1;
			list[1].childNodes[0].onclick();
		}
    } catch(ex) {
	}
}

var d = new Date();
var objSelect = document.getElementById("month");
for (var i = 0; i < objSelect.options.length; i++) {
    if (objSelect.options[i].value == d.getMonth() + 1) {
        objSelect.options[i].text = d.getMonth() + 1;
        objSelect.options[i].selected = true;
        break;
    }
}