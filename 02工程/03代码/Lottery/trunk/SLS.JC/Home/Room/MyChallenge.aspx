<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyChallenge.aspx.cs" Inherits="Home_Room_MyChallenge" %>

<%@ Register src="UserControls/UserMyIcaile.ascx" tagname="UserMyIcaile" tagprefix="uc2" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=_Site.Name %>-我的擂台-用户中心-<%=_Site.Name %>－买彩票，就上<%=_Site.Name %> ！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网<%=_Site.Url %>是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站!" />
    <meta name="keywords" content="合买,体育彩票,开奖信息." />
     
    <link href="Style/css.css" rel="stylesheet" type="text/css" />    
    <link rel="stylesheet" type="text/css" href="/Challenge/Style/ring.css" />
    <link rel="shortcut icon" href="../../favicon.ico" />
    <style type="text/css">
            .skover
            {
                background-color:#F8F8F8;
            }
            .skout
            {
            	background-color:MistyRose;            	
            }
    </style>
</head>
<body onload="showSameHeight();">
    <form id="form1" runat="server">
       <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
     <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" />
        </div>
        <div id="menu_right">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="40" height="30" align="right" valign="middle" class="red14">
                <img src="images/icon_6.gif" width="19" height="16" />
            </td>
            <td valign="middle" class="red14" style="padding-left: 10px;">
                我的擂台记录
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" background="images/zfb_left_bg_2.jpg">
        <tr>
            <td width="10" height="29" align="left">
                &nbsp;
            </td>
            <td width="100" id="tdHistory" align="center" background="images/admin_qh_100_2.jpg"
                onclick="clickTabMenu(this,'url(images/admin_qh_100_1.jpg)','myIcaileTab');ShowOrHiddenDiv('divHistory');"
                style="cursor: pointer;" class="blue12" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)">
                投注记录
            </td>
            <td width="6">
                &nbsp;
            </td>
            <td width="100" id="tdReward" align="center" background="images/admin_qh_100_2.jpg"
                onclick="clickTabMenu(this,'url(images/admin_qh_100_1.jpg)','myIcaileTab');ShowOrHiddenDiv('divReward');"
                style="cursor: pointer;" class="blue12" onmouseover="mOver(this,1)" onmouseout="mOver(this,2)">
                保存投注
            </td>
            <td width="6">
                &nbsp;
            </td>
            <td width="307" align="center" class="blue14">
                &nbsp;
            </td>
            <td width="307" class="black12">
                &nbsp;
            </td>
        </tr>
    </table>
   
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td height="1" colspan="9" bgcolor="#FFFFFF">
            </td>
        </tr>
        <tr>
            <td height="2" colspan="9" bgcolor="#6699CC">
            </td>
        </tr>
    </table>
    <table id="myIcaileTab" runat="server" width="100%" border="0"
        bgcolor="#DDDDDD">
        <tr>
            <td valign="top" style="background-color: White;">
                <div id="divHistory">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#DDDDDD" style="border-left: 1px solid #DDDDDD;
                        border-right: 1px solid #DDDDDD;">
                        <tr>
                            <td height="30" colspan="8" align="left" bgcolor="#F3F3F3" style="padding: 5px 10px 5px 2px;">
                                <strong>
                            &nbsp;擂台历史投注记录</strong>
                            </td>
                        </tr>
                    </table>
                    <!--投注记录部分-->
                        <div class="popuptab">
        <table class="table_c1" id="productTable" cellspacing="1" cellpadding="0" width="100%"
            align="center" style="border-style:solid; border-spacing:0px; border-color:#F8F8F8">
            <tbody>
                <tr class="topring_search">
                    <td height="33" colspan="6" style="text-align: left; padding-left: 10px; border-top: 1px solid #FDE2C6">
                        方案搜索
                        <select name="dropYear" id="dropYear">
                        </select>
                        <select name="dropMonth" id="dropMonth">
                            <option value="1">1月</option>
                            <option value="2">2月</option>
                            <option value="3">3月</option>
                            <option value="4">4月</option>
                            <option value="5">5月</option>
                            <option value="6">6月</option>
                            <option value="7">7月</option>
                            <option value="8">8月</option>
                            <option value="9">9月</option>
                            <option value="10">10月</option>
                            <option value="11">11月</option>
                            <option value="12">12月</option>
                        </select>
                        <input type="button" onclick="Search()" value="查 询" id="btnSearch" class="btn_ringso" />
                    </td>
                </tr>
                <tr>
                    <td width="13%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        日期
                    </td>
                    <td width="9%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        选择场数
                    </td>
                    <td width="10%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        过关
                    </td>
                    <td width="40%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        投注内容
                    </td>
                    <td width="8%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        积分
                    </td>
                    <td width="8%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        投注
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="divload" style="top: 50%; right: 50%; padding: 0px; margin: 0px; z-index: 999;clear:both;">
                       <center><img src="/Images/ajax-loader.gif" alt=""/></center>
        </div>
    </div>
                    <!--结束-->
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                        <tr>
                            <td width="368" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px;">
                                总积分累计： <span class="red12">
                                    <asp:Label ID="lblSumWinMoney" runat="server" Text="0"></asp:Label>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td valign="top" id="id1" style="background-color: White;">
                <div id="divReward" class="popuptab">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#DDDDDD" style="border-left: 1px solid #DDDDDD;
                        border-right: 1px solid #DDDDDD;">
                        <tr>
                            <td height="30" colspan="8" align="left" bgcolor="#F3F3F3" style="padding: 5px 10px 5px 2px;">
                                <strong>
                            &nbsp;</strong><strong>
                                擂台保存记录</strong>
                            </td>
                        </tr>
                    </table>
                    <!--保存记录开始-->
        <table class="table_c1" id="productTableSave" cellspacing="1" cellpadding="0" width="100%"
            align="center" border="0" style="border-style:solid; border-spacing:0px; border-color:#F8F8F8">
            <tbody> 
                <tr>
                    <td></td>
                </tr>
                <tr style="text-align:center; border-bottom: 1px solid #FDE2C6;">
                    <td width="13%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        方案日期
                    </td>
                    <td width="9%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        选择场数
                    </td>
                    <td width="10%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        玩法方式
                    </td>
                    <td width="40%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        保存方案内容
                    </td>
                    <td width="8%" style=" background-color:#E9F1F8; color:#0066BA; height:30px;">
                        投注
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="divloadSave" style="top: 50%; right: 50%; padding: 0px; margin: 0px; z-index: 999;clear:both;">
                       <center><img src="/Images/ajax-loader.gif" alt=""/></center>
        </div>
                    <!--保存记录结束-->                    
                    <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8" style="margin-top: 10px;">
                        <tr>
                            <td width="385" bgcolor="#F8F8F8" class="black12" style="padding: 5px 10px 5px 10px; text-align:left;" align="left">
                                保存方案总数： <span class="red12">
                                    <asp:Label ID="lblRewardCount" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdCurDiv" runat="server" value="divReward" />
      </div>
    </div> 
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    <!--投注记录-->
    <asp:HiddenField ID="userId" runat="server" Value="" />
    <asp:HiddenField ID="currentPageIndex" runat="server" Value="1" />
    </form>
    <script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    
        document.getElementById("dropYear").options.add(new Option(new Date().getFullYear(), new Date().getFullYear()));
        document.getElementById("dropYear").options.add(new Option(new Date().getFullYear() - 1, new Date().getFullYear() - 1));
        document.getElementById("dropMonth").selectedIndex = new Date().getMonth();
        
        function ShowOrHiddenDiv(id) {
            switch (id) {
                case 'divHistory':
                    document.getElementById("hdCurDiv").value = "divHistory";
                    break;
                case 'divReward':
                    document.getElementById("hdCurDiv").value = "divReward";
            }
        }

        function mOver(obj, type) {
            if (type == 1) {
                obj.style.textDecoration = "underline";
                obj.style.color = "#FF0065";
            }
            else {
                obj.style.textDecoration = "none";
                obj.style.color = "#226699";
            }
        }

        function showSameHeight() {
            if (document.getElementById("menu_left").clientHeight < document.getElementById("menu_right").clientHeight) {
                document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
            }
            else {
                if (document.getElementById("menu_right").offsetHeight >= 960) {
                    document.getElementById("menu_left").style.height = document.getElementById("menu_right").offsetHeight + "px";
                }
                else {
                    document.getElementById("menu_left").style.height = "960px";
                }
            }
            // 擂台记录
            Changepage(1);
            // 投注记录
            SaveSchemes(1);
        }

        function clickTabMenu(obj, backgroundImage, tabId) {
            switch (obj.id) {
                case 'tdHistory':
                    document.getElementById("divHistory").style.display = "";
                    document.getElementById("divReward").style.display = "none";
                    document.getElementById("tdHistory").style.background = "url(images/admin_qh_100_1.jpg)";
                    document.getElementById("tdReward").style.background = "url(images/admin_qh_100_2.jpg)";
                    break;
                case 'tdReward':
                    document.getElementById("divHistory").style.display = "none";
                    document.getElementById("divReward").style.display = "";
                    document.getElementById("tdReward").style.background = "url(images/admin_qh_100_1.jpg)";
                    document.getElementById("tdHistory").style.background = "url(images/admin_qh_100_2.jpg)";
                    break;
            }
        }

        // 擂台记录
        var index = 0;
        var state = false;
        window.$ = jQuery;
        Changepage = function(pageindex) {
            index = pageindex;
            if (state) {
                Home_Room_MyChallenge.GetPage($("#userId").val(), $("#dropYear").val(), $("#dropMonth").val(), pageindex, comeback);
            } else {
                Home_Room_MyChallenge.GetPage($("#userId").val(), '', '', pageindex, comeback);
            }
        }
        comeback = function(res) {
            if (res == "" || res == null) {
                $("#productTable tr:gt(1)").remove();
                $("#productTable").append("");
                return;
            }
            $("#divload").hide();
            $("#productTable tr:gt(1)").remove();
            $("#productTable").append(res.value);
            $("#productTable tr:gt(1):odd").attr("class", "sk1");
            $("#productTable tr:gt(1):even").attr("class", "skover");

            $("#productTable tr:gt(1):odd").mouseout(function() {
                this.bgColor = 'White';
            });
            $("#productTable tr:gt(1):odd").mouseover(function() {
                this.bgColor = 'MistyRose';
            });

            $("#productTable tr:gt(1):even").mouseout(function() {
                this.className = 'skover';
            });
            $("#productTable tr:gt(1):even").mouseover(function() {
                this.className = 'skout';
            });
            //onmouseout=\"JavaScript:this.bgColor='White';\" onmouseover=\"JavaScript:this.bgColor='MistyRose';\"
        }


        Search = function() {
            state = true;
            $("#divload").show();
            $("#productTable tr:gt(1)").remove();
            $("#productTable tr:gt(2)").remove();
            $("#productTable").append("");
            var year = $("#dropYear").val()
            var month = $("#dropMonth").val()
            Home_Room_MyChallenge.GetPage($("#userId").val(), year, month, 1, comeback);
        }

        // 保存记录
        var index_Save = 0;
        var state_Save = false;
        SaveSchemes = function(pageindex) {
            index_Save = pageindex;
            document.getElementById("dropMonth").selectedIndex = new Date().getMonth();
            if (state) {
                Home_Room_MyChallenge.GetPages_Save($("#userId").val(), pageindex, comeback_Save);
            } else {
                Home_Room_MyChallenge.GetPages_Save($("#userId").val(), pageindex, comeback_Save);
            }
        }
        comeback_Save = function(res) {
            if (res == "" || res == null) {
                $("#productTableSave tr:gt(1)").remove();
                $("#productTableSave").append("");
                return;
            }
            $("#divloadSave").hide();
            $("#productTableSave tr:gt(1)").remove();
            $("#productTableSave").append(res.value);
            $("#productTableSave tr:gt(1):odd").attr("class", "sk1");
            $("#productTableSave tr:gt(1):even").attr("class", "skover");

            $("#productTableSave tr:gt(1):odd").mouseout(function() {
                this.bgColor = 'White';
            });
            $("#productTableSave tr:gt(1):odd").mouseover(function() {
                this.bgColor = 'MistyRose';
            });

            $("#productTableSave tr:gt(1):even").mouseout(function() {
                this.className = 'skover';
            });
            $("#productTableSave tr:gt(1):even").mouseover(function() {
                this.className = 'skout';
            });
            //onmouseout=\"JavaScript:this.bgColor='White';\" onmouseover=\"JavaScript:this.bgColor='MistyRose';\"
        }
        
    </script>
    <script type="text/javascript">
        ShowOrHiddenDiv("divReward");
        clickTabMenu(document.getElementById("tdHistory"), 'url(images/admin_qh_100_1.jpg)', 'myIcaileTab');
    </script>

    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
