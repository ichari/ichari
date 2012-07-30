<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%
	int redom = (int)(Math.random()*100)%12 ;
	int angle = 360*5 + redom * 30 ;
	System.out.println(redom+"-----------------------------"+angle);
%>
<%="{\"massage\":\"摇奖成功ssss\",\"status\":1,\"target\":\"_blank\",\"type\":\"checkLogin\",\"angle\":"+angle+",\"url\":\"http://www.baidu.com\"}" %>
