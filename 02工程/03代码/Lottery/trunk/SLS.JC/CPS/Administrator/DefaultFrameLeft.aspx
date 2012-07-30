<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefaultFrameLeft.aspx.cs" Inherits="Cps_Administrator_DefaultFrameLeft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    <!--
    a {
	    font-size: 12px;
	    color: #103875;
    }
    a:link {
	    text-decoration: none;
	    color: #103875;
    }
    a:visited {
	    text-decoration: none;
	    color: #103875;
    }
    a:hover {
	    text-decoration: none;
	    color: #FF6600;
    }
    a:active {
	    text-decoration: none;
	    color: #103875;
    }
    -->
    </style>

</head>
<body>
    <form id="form1" runat="server" style="margin: 0px; padding: 0px">

        <table  style="font-size: 12px; text-decoration: none">
            <tr>
                <td  id="LeftMenu" width="150px" valign="top"> 
                    <table  border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC" id="CpsBusiness" visible="true"
                        runat="server" style="margin-top: 5px">
                        <tr style="display: " id="trManager">
                            <td valign="top" bgcolor="#FFFFFF" style="padding: 5px;">
                            
                                <table id="MenuTable1" width="150px" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC" 
                                    runat="server" style="margin-top: 6px">
                                    <tr>
                                        <td height="30" background="../images/bg_hui_33.gif" bgcolor="#F5F5F5" 
                                            style="font-size: 14px; font-weight: bold; text-align: center; color: #000000;">
                                            <a href="javascript:;" onclick="showMenu(this);return false;" 
                                                style="color: #000000">商家管理</a>
                                        </td>
                                    </tr>
                                    <tr id="ParentMenu1" > 
                                        <td valign="top" bgcolor="#FFFFFF" style="padding: 10px;">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                        <a href="CpsPromoterList.aspx"  target="mainFrame" style="text-decoration: none">
                                                            推广员列表</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                        <a href="CpsAgentList.aspx" target="mainFrame"  style="text-decoration: none"> 
                                                            代理商一览表</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                </table>
                            
                                <table id="Table1" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC" 
                                    runat="server" style="margin-top: 6px">
                                    <tr >
                                        <td height="30" background="../images/bg_hui_33.gif" bgcolor="#F5F5F5" 
                                            style="padding-left: 5px;font-size: 14px; font-weight: bold; text-align: center; color: #000000;">
                                            <a href="javascript:;" onclick="showMenu(this);return false;">加盟申请管理</a>
                                        </td>
                                    </tr>
                                    <tr  id="ParentMenu2" style="display: none" >
                                        <td valign="top" bgcolor="#FFFFFF" style="padding: 10px;">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"align="center">
                                                        <a href="cpstry.aspx" target="mainFrame"  style="text-decoration: none">
                                                            处理代理商申请</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                                <table id="Table2" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC" 
                                    runat="server" style="margin-top: 6px">
                                    <tr  >
                                        <td height="30" background="../images/bg_hui_33.gif" bgcolor="#F5F5F5" 
                                            style="padding-left: 5px;font-size: 14px; font-weight: bold; text-align: center; color: #000000;">
                                            <a href="javascript:;" onclick="showMenu(this);return false;">佣金提款管理</a>
                                        </td>
                                    </tr>
                                    <tr id="ParentMenu3" style="display: none">
                                        <td valign="top" bgcolor="#FFFFFF" style="padding: 10px;">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                        <a href="UserDistill.aspx"  target="mainFrame" style="text-decoration: none">
                                                            处理用户提款申请</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                        <a href="UserDistillWaitPay.aspx"  target="mainFrame" style="text-decoration: none">
                                                             待付款佣金列表</a>
                                                    </td>
                                                </tr>
                                            </table>
                                             <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                         <a href="UserDistillPayed.aspx"  target="mainFrame" style="text-decoration: none">
                                                         已付款佣金列表</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                         <a href="BonusPayOff.aspx"  target="mainFrame" style="text-decoration: none">
                                                         佣金发放通知</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                         <a href="MonthTradeSum.aspx"  target="mainFrame" style="text-decoration: none">
                                                         CPS月度结算表</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="display:none;"><%--cps业务细表先隐藏--%>
                                    <tr>
                                        <td width="92%" height="28"  
                                            style="padding-left: 5px; background-image: url('../../../Images/bg_td_2.jpg'); background-repeat: no-repeat;" >
                                            <a href="FinanceCps.aspx"   target="mainFrame" style="text-decoration: none">
                                                联盟推广业务明细表</a>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC" id="DivCPSNews"
                                    runat="server" style="margin-top: 6px">
                                    <tr >
                                        <td height="30" background="../images/bg_hui_33.gif" bgcolor="#F5F5F5" 
                                            style="padding-left: 5px;font-size: 14px; font-weight: bold; text-align: center; color: #000000;">
                                            <a href="javascript:;" onclick="showMenu(this);return false;">CPS新闻公告管理</a>
                                        </td>
                                    </tr>
                                    <tr id="ParentMenu4" style="display: none" >
                                        <td valign="top" bgcolor="#FFFFFF" style="padding: 10px;">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                        <a href="News.aspx?Type=1"   target="mainFrame"  style="text-decoration: none">
                                                            新闻公告</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                        <a href="News.aspx?Type=2" target="mainFrame" style="text-decoration: none">
                                                            推广指南</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC" id="Table4"
                                    runat="server" style="margin-top: 6px">
                                    <tr >
                                        <td height="30" background="../images/bg_hui_33.gif" bgcolor="#F5F5F5" 
                                            style="padding-left: 5px;font-size: 14px; font-weight: bold; text-align: center; color: #000000;">
                                            <a href="javascript:;" onclick="showMenu(this);return false;">CPS商家注册协议</a>
                                        </td>
                                    </tr>
                                    <tr id="ParentMenu5" style="display: none" >
                                        <td valign="top" bgcolor="#FFFFFF" style="padding: 10px;">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;"  align="center">
                                                        <a href="CPSRegisterAgreement.aspx" target="mainFrame" style="text-decoration: none">
                                                            CPS商家注册协议</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>  
                                <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC" id="Table3"
                                    runat="server" style="margin-top: 6px">
                                    <tr >
                                        <td height="30" background="../images/bg_hui_33.gif" bgcolor="#F5F5F5" 
                                            style="padding-left: 5px;font-size: 14px; font-weight: bold; text-align: center; color: #000000;">
                                            <a href="javascript:;" onclick="showMenu(this);return false;">系统设置管理</a>
                                        </td>
                                    </tr>
                                    <tr id="ParentMenu6" style="display: none" >
                                        <td valign="top" bgcolor="#FFFFFF" style="padding: 10px;">
                                        <%--    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;" align="center">
                                                        <a href="SystemSetup.aspx" target="mainFrame"  style="text-decoration: none">
                                                            系统设置</a>
                                                    </td>
                                                </tr>
                                            </table>--%>
                                             <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="92%" height="28"  style="padding-left: 5px;" align="center">
                                                        <a href="BonusScaleSetup.aspx" target="mainFrame"  style="text-decoration: none">
                                                            推广佣金比例设置</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                            
                            </td>
                        </tr>
                    </table>
                </td>
                <td >
                    <img src="../Images/btnExpand1.jpg" alt="收缩/展开" onclick="ExpandLeftMenu(this)" 
                        style="cursor: pointer" />
                </td>
            </tr>
         </table>
        <script language="javascript" type="text/javascript">


            //显示顶级工菜单
            var lastTR = null;
            function showMenu(aOjb)
            {

                if (lastTR != null)
                {
                    lastTR.style.display = "none";
                }
                try
                {
                    document.getElementById("ParentMenu1").style.display = "none";
                    document.getElementById("ParentMenu2").style.display = "none";
                    document.getElementById("ParentMenu3").style.display = "none";
                    document.getElementById("ParentMenu4").style.display = "none";
                    document.getElementById("ParentMenu5").style.display = "none";
                    document.getElementById("ParentMenu6").style.display = "none";
                }
                catch (ex) { alert(ex.message); }
                try
                {
                    var tr = aOjb.parentNode.parentNode.nextSibling;
                    tr.style.display = "";

                    var LeftMenuTD = lastTR = document.getElementById("LeftMenu");
                    LeftMenuTD.style.display = "";
                }
                catch (ex) { alert(ex.message); }

            }

            var lastMenuTD = null
            function hightlightSelectItem(aOjb)//aOjb为表格中的<A>
            {
                try
                {
                    if (lastMenuTD != null)
                    {
                        lastMenuTD.style.backgroundImage = "";
                    }
                    var td = aOjb.parentNode;
                    lastMenuTD = td;
                    //alert(td.style);
                    td.style.backgroundImage = "url('../Images/bg_td_2.jpg')";
                }
                catch (ex)
                {
                    alert(ex.message);
                }

            }

            //展开初始菜单和打开初始页面
            lastTR = document.getElementById("ParentMenu1");


            function ExpandLeftMenu(senderOjb)
            {
                try
                {
                    var LeftMenuTD = lastTR = document.getElementById("LeftMenu");
                    if (LeftMenuTD.style.display == "none")
                    {
                        //显示
                        LeftMenuTD.style.display = "";
                        senderOjb.src = "../Images/btnExpand1.jpg"
                        window.parent.ClientFrameset.cols = "210,*";
                    }
                    else
                    {
                        //隐藏
                        LeftMenuTD.style.display = "none";
                        senderOjb.src = "../Images/btnExpand2.jpg"
                        window.parent.ClientFrameset.cols = "50,*";
                    }
                }
                catch(ex)
                {
                    alert(ex.message)
                }
            }
            
            
        </script>
    </form>
</body>
</html>
