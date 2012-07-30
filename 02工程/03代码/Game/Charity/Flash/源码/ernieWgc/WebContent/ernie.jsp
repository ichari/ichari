<%@ page language="java" import="java.util.*" pageEncoding="UTF-8"%>
<%
String path = request.getContextPath();
String basePath = request.getScheme()+"://"+request.getServerName()+":"+request.getServerPort()+path+"/";
%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    <title>My JSP 'index.jsp' starting page</title>
	<meta http-equiv="pragma" content="no-cache">
	<meta http-equiv="cache-control" content="no-cache">
	<meta http-equiv="expires" content="0">    
	<meta http-equiv="keywords" content="keyword1,keyword2,keyword3">
	<meta http-equiv="description" content="This is my page">
	<!--
	<link rel="stylesheet" type="text/css" href="styles.css">
	-->
  </head>
  
  <body>
    <div align="center">
		<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"
	            id="ernie" width="690" height="690"
	             codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab">
	         <param name="movie" value="<%=basePath%>/ernie/ernieWgc.swf" />
	         <param name="quality" value="high" />
	         <param name="wmode" value="transparent" />
	         <param name="allowScriptAccess" value="sameDomain" />
	         <param name="flashvars" value="uId=1&aId=1&baseUrl=<%=basePath%>" />
	         <embed src="<%=basePath%>/ernie/ernieWgc.swf?uId=1&aId=1&baseUrl=<%=basePath%>" quality="high" wmode="transparent"
	             width="690" height="690" name="ernie" align="middle"
	             play="true" loop="false" quality="high" allowScriptAccess="sameDomain"
	             type="application/x-shockwave-flash"
	             pluginspage="http://www.macromedia.com/go/getflashplayer">
	         </embed>
	     </object>
	 </div>
  </body>
  
  <script type="text/javascript">
  	var flashObj = thisMovie("ernie")
  	function thisMovie(movieName) {
		if(navigator.appName.indexOf("Microsoft") != -1){
            return window[movieName];
        } else {
            return document[movieName];
        }
    }
  	
  	
  	/**
  	*向flex发送请求  调用flex方法
  	**/
  	function sendErnie(){
  		flashObj.sendErnie();
  	}
  	
  	/**
  	*flex调用js 处理flex结束后结果
  	*/
  	function asCallJsEndErnie(massage,url,target){
  		alert("massage = "+massage+"\nurl="+url+"\ntarget="+target);
  		//将摇奖指针还原
  		flashObj.ernieGotoRotation();
  	}
  </script>
</html>
