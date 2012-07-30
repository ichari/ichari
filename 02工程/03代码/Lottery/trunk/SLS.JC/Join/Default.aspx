<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Join_Default" %>

<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>喜彩网彩票-合买跟单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/hemai.css" rel="stylesheet" type="text/css" />
    <link href="Style/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <uc1:WebHead ID="WebHead1" runat="server" />
<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
  <div class="bsc6s">
    	<div class="bsc6">
            <div class="title bg1x bgp3 afff"><strong>推荐方案</strong></div>
            <p><a href="Project_List.aspx?id=5"><img src="/Images/_New/ben_09.png" alt="" width="105" height="122"></a></p>
            <p><a href="Project_List.aspx?id=6"><img src="/Images/_New/ben_10.png" alt="" width="105" height="122"></a></p>
            <p><a href="Project_List.aspx?id=13"><img src="/Images/_New/ben_11.png" alt="" width="116" height="120"></a></p>
            <p><a href="Project_List.aspx?id=28"><img src="/Images/_New/ben_12.png" alt="" width="130" height="120"></a></p>        
        </div>
        <div class="bsc7s">
  <!--合买名人-->
        <div class="main_mrhm">
            <div class="m_tit">
                <div class="bg1x bgp5">
                    <span style="font-weight:bold" class="bg1x bgp4">名人合买</span>
                </div>
            </div>
            <div class="mrhm_list">
                    <%=innerHTML%>
            </div>
        </div>
        <!--合买推荐-->
        <div class="h_main">
            <div class="TabTitle bg1x bgp5">
                <ul id="Ul1">
                    <li class="active  bg1x bgp1">合买推荐</li>
                </ul>
                <dl class="more_hemai">
                    <!---a href="/Join/Project_List.aspx?id=72">查看更多合买&gt;&gt;</a---></dl>
            </div>
            <!--方案表-->
            <div class="TabContent">
                <!--合买推荐-->
               <div id="myTab0_Content0">
                    <div class="cpTitle">
                        <ul id="cpTab0">
                            <li class="hidden" mid="74">足彩胜负</li>
                            <dt class="hemai_line hidden"></dt>
                            <li mid="75" class="hidden">任选9场</li>
                            <dt class="hemai_line hidden"></dt>
                            <li mid="72" class="hidden">竞彩足球</li>
                            <dt class="hemai_line hidden"></dt>
                            <li mid="73" class="hidden">竞彩篮球</li>
                            <dt class="hemai_line hidden"></dt>
                            <li mid="15" class="hidden">六场半全场</li>
                            <dt class="hemai_line hidden"></dt>
                            <li mid="2" class="hidden">进球彩</li>
                            <dt class="hemai_line hidden"></dt>
                            <li mid="5" class="active">双色球</li>
                            <dt class="hemai_line"></dt>
                            <li mid="28">时时彩</li>
                            <dt class="hemai_line"></dt>
                            <li mid="6">福彩3D</li>
                            <dt class="hemai_line"></dt>
                            <li mid="13">七乐彩</li>
                        </ul>
                    </div>
                    <div class="cpContent">
                      <table width="100%" cellspacing="0" cellpadding="0" class="tab_hemai" id="SchemeList">
                            <thead>
                                <tr>
                                    <th>
                                        序号
                                    </th>
                                    <th>
                                        发起人
                                    </th>
                                    <th>
                                        发起人战绩<span id="Level"></span>
                                    </th>
                                                                        <th>
                                        倍数
                                    </th>
                                    <th>
                                        方案金额<span id="Money"></span>
                                    </th>
                                    <th>
                                        每份金额
                                    </th>
                                                                        <th>
                                        方案进度<span id="Schedule"></span>
                                    </th>
                                    <th>
                                        剩余份数
                                    </th>
                                    <th>
                                        认购份数
                                    </th>
                                    <th>
                                        操作
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                       <div id="divload" style="top: 50%; right: 50%; padding: 0px; margin: 0px; z-index: 999">
                            <img src="Images/ajax-loader.gif" alt=""/>正在加载方案，请稍后...
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>
<uc2:WebFoot ID="WebFoot1" runat="server" />
<script src="JScript/ScrollPic.js" type="text/javascript"></script>
<script src="JScript/ProjectList.js" type="text/javascript"></script>
<script type="text/javascript" src="JScript/PaginationSchemeList.js"></script>
</form>
</body>
</html>
