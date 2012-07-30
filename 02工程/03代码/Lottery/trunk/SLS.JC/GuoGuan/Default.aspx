<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="GuoGuan_SFC_Default" %>

<%@ Register Src="../Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="../Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>过关统计</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/statistics.css" type="text/css" rel="stylesheet" />
    <link href="Style/hemai.css" rel="stylesheet" type="text/css" />
    <link href="Style/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <uc1:WebHead ID="WebHead1" runat="server" />
    <div class="wrap">
        <!--过关统计sider-->
        <div class="side_sta">
            <div class="sta_nav">
                <h2>
                    <img alt="" src="images/stanav.gif" width="149" height="29" /></h2>
                <h3 class="bb">
                    竞彩足球</h3>
                <ul>
                    <li class="curr" mid="7201"><a href="#">胜平负</a> </li>
                    <li mid="7202"><a href="#">比分</a> </li>
                    <li mid="7203"><a href="#">进球数</a> </li>
                    <li mid="7204"><a href="#">半全场</a> </li>
                </ul>
                <div class="cl">
                </div>
            </div>
        </div>
        <!--过关统计main-->
        <div class="main_sta">
            <div class="sta_toptab">
                <h2 class="sta_nav1">
                    <!--对应1~8-->
                </h2>
                <dl class="sta_topscreen">
                    <span class="zi"></span><span class="xx"></span><span class="zi"></span>
                    <span><asp:DropDownList ID="ddl_Day" runat="server"></asp:DropDownList> </span>
                    <span class="xx">
                        <input type="text" id="SerachCondition" Width="180px" MaxLength="25" onblur="if(this.value==''){this.value='输入用户名或方案号';this.className='c_999'}" class="c_999" onfocus="if(this.value=='输入用户名或方案号'){this.value='';this.className='c_333'};" autocomplete="off" value="输入用户名或方案号"></asp:TextBox>
                    </span><span>
                        <input type="button" id="srearch" name="srearch" class="btn_staso"/>
                    </span>
                </dl>
            </div>
            <!--tab-->
            <div class="statab">
                <div class="statab_nav">
                    <ul>
                        <li class="curr">
                            过关统计</li>
                    </ul>
                </div>
                <div class="statab_cont">
                    <table class="tab_hemai"  id="SchemeList" width="839px" border="0">
                        <thead>
                            <tr>
                                <%--<th width="10%">方案号</th>--%>
                                <th width="13%">发起人</th>
                                <th width="9%">注数</th>
                                <th width="10%">选择场次</th>
                                <th width="17%">过关方式</th>
                                <th width="7%">中奖注数</th>
                                <th width="9%">命中场次</th>
                                <th width="7%">正确率</th>
                                <th width="9%">中奖金额</th>
                                <th width="10%">类别</th>
                                <th width="9%">当前方案</th>
                            </tr>
                            </thead>
                    </table>
                    <div id="divload" style="top: 50%; right: 50%; padding: 0px; margin: 0px; z-index: 999">
                        <img src="Images/ajax-loader.gif" alt=""/>
                    </div>
                </div>
               <div>
                    <div class="numb" id="EachPageNum">
                        单页方案个数：<a title="" style="cursor: pointer;" class="current">20</a><a
                            title="" style="cursor: pointer;">30</a><a title="" style="cursor: pointer;">40</a></div>
                    <div style="float:right;padding-bottom: 3px; padding-top: 3px;">
                        <span><input id="govalue" name="Btn_Go" type="text" style="width:30px;" /><input type="button" id="Btn_Go" value="GO" class="goto" /></span></div>
                        <div id="Pagination" class="yahoo2" style="width: auto; float:right;">
                    </div>
                </div>
                <div class="cl">
                    </div>
            </div>
        </div>
        <div class="cl">
        </div>
    </div>
    <div class="botpic">
        <uc2:WebFoot runat="server" ID="foot" />
    </div>
    <input type="hidden" id="hidPlayTypeID" name ="hidPlayTypeID" value="7201"/>
    <script src="../JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="JScript/Pagination.js" type="text/javascript"></script>
    <script src="JScript/ProjectList.js" type="text/javascript"></script>
    <script src="/Challenge/JScript/public.js" type="text/javascript"></script>
    </form>
</body>
</html>
