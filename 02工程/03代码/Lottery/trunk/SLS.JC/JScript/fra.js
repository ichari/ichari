function openmodaldialog(url, width, height)
{
	var p = "status:no; center:yes; help:no; minimize:yes; maximize:no; dialogWidth:"+width+"px; scroll:no;dialogHeight:"+height+"px";
	return window.showModalDialog(url, null, p);
}

function f_frameStyleResize(targObj)
{ //try{document.domain = location.host.replace(/^[^\.]+\./,'')}catch(e){};var targWin = targObj.parent.parent.document.all[targObj.name];
	var targWin = targObj.parent.document.getElementById(targObj.name);
	if(targWin != null) 
	{
		var HeightValue = targObj.document.body.scrollHeight
		if(HeightValue < 270){HeightValue = 317}
		targWin.style.height = HeightValue+"px";
	}
}
	
function f_iframeResize()
{
	bLoadComplete = true;
	f_frameStyleResize(self);
}
var bLoadComplete = false;
window.onload = f_iframeResize;