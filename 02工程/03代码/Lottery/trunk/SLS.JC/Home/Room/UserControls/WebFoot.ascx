<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebFoot.ascx.cs" Inherits="Home_Room_UserControls_WebFoot" %>
<!--footer-->
<div class="footer">
	<%--<div class="bot_nav"><a href="/UserReg.aspx" target="_blank">用户注册</a> | <a href="/BotNav/zz1.html">资质荣誉</a> | <a href="/BotNav/contact.html">联系方式</a> | <a href="/BotNav/job.html">人才招聘</a> | <a href="/BotNav/zlhz.html">战略合作</a> | <a href="/SiteMap.html">站点地图</a> | <a href="/BotNav/zlhz.html">友情链接</a></div>--%>
    <div class="bot_copyright">
    	版权所有：<%= _Site.Company %> ICP备案：<a href="http://www.miibeian.gov.cn/" style="color:#999999;" target="_blank">粤ICP备08000656号</a>
        <span style="text-decoration:none;margin:0 10px 0 10px;"><a href="#">关于我们</a> | <a href="#">联系我们</a></span><br />
        <%--公司地址：<%=_Site.Address%>   客服热线：<%= _Site.ServiceTelephone %>   邮政编码：<%= _Site.PostCode %> <br />--%>
        <%= _Site.Name %> 郑重提示：彩票有风险，投注需谨慎 不向未满18周岁的青少年出售彩票！
    </div>
    <div class="bot_linklogo_mrt" style="display:none;">
    	<ul>
        	<li class="b_link01"><a href="#">体彩中心</a></li>
            <li class="b_link02"><a href="#">网络110</a></li>
            <li class="b_link04"><a href="#">网络110</a></li>
        </ul>
    </div>
    <div class="cl"></div>
</div>

<!-- foot -->
<script type="text/javascript">
    $('#endft').load('<%= System.Configuration.ConfigurationManager.AppSettings["JsHeaderUrl"] %>/home/bottom');
</script>
<span id="endft"></span>
