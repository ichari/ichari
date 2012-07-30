<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Buy_Luck_Buy.aspx.cs" Inherits="Buy_Luck_Buy" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>幸运数字</title>

    <script src="../JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../JScript/Public.js" type="text/javascript"></script>
    <script src="JScript/Buy_Luck_Buy.js" type="text/javascript"></script>

    <link href="../Style/lucknumber.css" rel="stylesheet" type="text/css" />    
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link href="style/Buy.css" rel="stylesheet" type="text/css" />

    <link rel="shortcut icon" href="../favicon.ico" />
    <style type="text/css">
        html
        {
            overflow-y: scroll;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="HidUserID" runat="server" Value="-1" /><div
            style="display: none;">
            <iframe id="Login_Iframe" name="Login_Iframe" width="100%" frameborder="0" scrolling="no"
                src="../Home/Room/UserLoginDialog.aspx"></iframe>
        </div>
<%--        <div style="display: none;">
            <uc1:WebHead ID="WebHead1" runat="server" />
        </div>--%>
        <table width="800" border="0" cellspacing="0" cellpadding="0" height="82" class="top">
            <tr>
                <td width="242">
                    <img src="../images/t_logo_01.gif" width="242" height="82" />
                </td>
                <td width="333">
                    &nbsp;
                </td>
                <td width="96">
                    &nbsp;
                </td>
                <td width="76">
                    &nbsp;
                </td>
                <td width="25">
                    &nbsp;
                </td>
                <td width="28">
                    &nbsp;
                </td>
            </tr>
        </table>
        <table width="800" border="0" cellspacing="0" cellpadding="0" height="105" style="margin: 0 auto;
            background-color: #e9f3fc; border-left: #6aa4d7 solid 1px; border-right: #6aa4d7 solid 1px;">
            <tr>
                <td width="132">
                    &nbsp;
                </td>
                <td width="134" align="center">
                    <a href="javascript:;" id="hrefSX" onclick="return ClickXYJX('sx',2)">
                        <img src="../images/fenlei_12.gif" width="88" height="85" /></a>
                </td>
                <td width="133" align="center">
                    <a href="javascript:;" id="hrefCSRQ" onclick="return ClickXYJX('csrq',3)">
                        <img src="../images/fenlei_14.gif" width="88" height="85" /></a>
                </td>
                <td width="133" align="center">
                    <a href="javascript:;" id="hrefXZ" onclick="return ClickXYJX('xz',1)">
                        <img src="../images/fenlei_16.gif" width="88" height="85" /></a>
                </td>
                <td width="133" align="center">
                    <a href="javascript:;" id="hrefXM" onclick="return ClickXYJX('xm',4)">
                        <img src="../images/fenlei_18.gif" width="88" height="85" /></a>
                </td>
                <td width="135">
                    &nbsp;
                </td>
            </tr>
        </table>
        <div class="container">
            <div class="tab_top">
                <div class="tab_topSub">
                    <img src="../images/tab_top_03.gif" width="10" height="31" /></div>
                <ul id="tab">
                    <li id="CJDLT" class="li_hover" onclick="InitLuckLotteryNumber_CJDLT();"><a href="javascript:void(0);">
                        超级大乐透</a></li>
                    <li id="PL3" onclick="InitLuckLotteryNumber_PL3();"><a href="javascript:void(0);">排列3</a></li>
                    <li id="SSQ" onclick="InitLuckLotteryNumber_SSQ();"><a href="javascript:void(0);">双色球</a></li>
                    <li id="3D" onclick="InitLuckLotteryNumber_3D();"><a href="javascript:void(0);">3 D</a></li>
                    <li id="QLC" onclick="InitLuckLotteryNumber_QLC();"><a href="javascript:void(0);">七乐彩</a></li>
                    <li id="PL5" onclick="InitLuckLotteryNumber_PL5();"><a href="javascript:void(0);">排列5</a></li>
                    <li id="QXC" onclick="InitLuckLotteryNumber_QXC();"><a href="javascript:void(0);">七星彩</a></li>
                    <li id="22X5" onclick="InitLuckLotteryNumber_22X5();"><a href="javascript:void(0);">
                        22选5</a></li>
                    <li id="31X7" onclick="InitLuckLotteryNumber_31X7();"><a href="javascript:void(0);">
                        31选7</a></li>
                </ul>
                <div class="tab_topSub">
                    <img src="../images/tab_top_09.gif" width="10" height="31" /></div>
            </div>
            <div class="main">
                <div class="div_15">
                </div>
                <div id="xxk">
                    <div id="sx">
                        <table width="92%" border="0" cellpadding="0" cellspacing="0" class="main_table">
                            <tr>
                                <td width="13%">
                                    <a href="javascript:setoption(this,1);">
                                        <img src="../images/sx_03.gif" alt="鼠" width="72" height="70" /></a>
                                </td>
                                <td width="13%">
                                    <a href="javascript:setoption(this,2);">
                                        <img src="../images/sx_05.gif" alt="牛" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,3);">
                                        <img src="../images/sx_07.gif" alt="虎" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,4);">
                                        <img src="../images/sx_09.gif" alt="兔" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,5);">
                                        <img src="../images/sx_11.gif" alt="龙" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,6);">
                                        <img src="../images/sx_13.gif" alt="蛇" width="72" height="70" /></a>
                                </td>
                                <td width="13%">
                                    <a href="javascript:setoption(this,7);">
                                        <img src="../images/sx_15.gif" alt="马" width="72" height="70" /></a>
                                </td>
                                <td width="13%">
                                    <a href="javascript:setoption(this,8);">
                                        <img src="../images/sx_17.gif" alt="羊" width="72" height="70" /></a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:setoption(this,1);">鼠</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,2);">牛</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,3);">虎</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,4);">兔</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,5);">龙</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,6);">蛇</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,7);">马</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,8);">羊</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:setoption(this,9);">
                                        <img src="../images/sx_19.gif" alt="猴" width="72" height="70" /></a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,10);">
                                        <img src="../images/sx_31.gif" alt="鸡" width="72" height="70" /></a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,11);">
                                        <img src="../images/sx_33.gif" alt="狗" width="72" height="70" /></a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,12);">
                                        <img src="../images/sx_35.gif" alt="猪" width="72" height="70" /></a>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:setoption(this,9);">猴</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,10);">鸡</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,11);">狗</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,12);">猪</a>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="csrq" style="display: none;">
                        <div style="padding-left: 20px;">
                            出生日期：<asp:TextBox ID="tbDate" runat="server" Width="80" CssClass="hui12" ToolTip="格式：1980-01-01"
                                Text="1980-01-01" onfocus="this.className='';this.value='';" onblur="if(this.value==''){this.className='hui12'; this.value='1980-01-01';}"></asp:TextBox></div>
                    </div>
                    <div id="xz" style="display: none;">
                        <table width="92%" border="0" cellpadding="0" cellspacing="0" class="main_table">
                            <tr>
                                <td width="13%">
                                    <a href="javascript:setoption(this,1);">
                                        <img src="../images/astro_01.gif" alt="白羊座" width="72" height="70" /></a>
                                </td>
                                <td width="13%">
                                    <a href="javascript:setoption(this,2);">
                                        <img src="../images/astro_02.gif" alt="金牛座" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,3);">
                                        <img src="../images/astro_03.gif" alt="双子座" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,4);">
                                        <img src="../images/astro_04.gif" alt="巨蟹座" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,5);">
                                        <img src="../images/astro_05.gif" alt="狮子座" width="72" height="70" /></a>
                                </td>
                                <td width="12%">
                                    <a href="javascript:setoption(this,6);">
                                        <img src="../images/astro_06.gif" alt="处女座" width="72" height="70" /></a>
                                </td>
                                <td width="13%">
                                    <a href="javascript:setoption(this,7);">
                                        <img src="../images/astro_07.gif" alt="天秤座" width="72" height="70" /></a>
                                </td>
                                <td width="13%">
                                    <a href="javascript:setoption(this,8);">
                                        <img src="../images/astro_08.gif" alt="天蝎座" width="72" height="70" /></a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:setoption(this,1);">白羊座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,2);">金牛座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,3);">双子座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,4);">巨蟹座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,5);">狮子座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,6);">处女座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,7);">天秤座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,8);">天蝎座</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:setoption(this,9);">
                                        <img src="../images/astro_09.gif" alt="射手座" width="72" height="70" /></a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,10);">
                                        <img src="../images/astro_10.gif" alt="摩羯座" width="72" height="70" /></a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,11);">
                                        <img src="../images/astro_11.gif" alt="水瓶座" width="72" height="70" /></a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,12);">
                                        <img src="../images/astro_12.gif" alt="双鱼座" width="72" height="70" /></a>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:setoption(this,9);">射手座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,10);">摩羯座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,11);">水瓶座</a>
                                </td>
                                <td>
                                    <a href="javascript:setoption(this,12);">双鱼座</a>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="xm" style="display: none;">
                        <div style="padding-left: 20px;">
                            姓名：
                            <asp:TextBox ID="tbName" runat="server" Width="80" CssClass="hui12" ToolTip="支持中英文"
                                Text="支持中英文" onfocus="if(this.value=='支持中英文') {this.className='';this.value='';}"
                                onblur="if(this.value=='') {this.className='hui12';this.value='支持中英文';}"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="main_bt">
                <img src="../images/main_bt_03.gif" width="790" /></div>
            <div class="div_25" style="height: 70px; margin-bottom: 10px;">
                <table style="margin: 0 auto;">
                    <tr>
                        <td style="width: 200px;" runat="server" id="tdLuckNumber">
                        </td>
                        <td style="text-align: center; padding-top: 10px;">
                            <img src="../Home/room/Images/ssq_bt_1.jpg" width="100" height="21" alt="" border="0" id="GetNum"
                                style="cursor: pointer" onclick="return GetLuckNumber();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <ShoveWebUI:ShoveConfirmButton ID="btn_OK" Style="cursor: pointer; background-image: url(../Home/room/Images/ssq_bt_2.jpg);
                                border-width: 0;" runat="server" Width="140px" Height="30px" CausesValidation="False"
                                OnClientClick="return CreateLogin('btn_OKClick();');" OnClick="btn_OK_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="div_25">
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HidlotteryName" runat="server" />
    <asp:HiddenField ID="HidType" runat="server" Value="1" />
    <asp:HiddenField ID="HidLuckNumber" runat="server" />
    <input id="tb_LotteryNumber" name="tb_LotteryNumber" type="hidden" />
    <asp:HiddenField ID="ddlSX" runat="server" Value="1" />
    <asp:HiddenField ID="ddlXiZuo" runat="server" Value="1" />
    <asp:HiddenField ID="HidLotteryID" runat="server" Value="-1" />
    <asp:HiddenField ID="HidIsuseID" runat="server" Value="-1" />
    <asp:HiddenField ID="HidIsuseEndTime" runat="server" Value="1" />
    <asp:HiddenField ID="tbPlayTypeID" runat="server" Value="-1" />
    <input id="HidPrice" name="HidPrice" type="hidden" value="2" />
    <input id="tbPlayTypeName" name="tbPlayTypeName" type="hidden" value="单式" />
    <input id="tb_hide_SumMoney" name="tb_hide_SumMoney" type="hidden" />
    <input id="tb_hide_AssureMoney" name="tb_hide_AssureMoney" type="hidden" />
    <input id="tb_hide_SumNum" name="tb_hide_SumNum" type="hidden" />
    <input id="HidMaxTimes" name="HidMaxTimes" type="hidden" value="1000" />
    
<script type="text/javascript" language="javascript">
    function CreateLogin(script) {
        if (Number(document.getElementById("<%=HidUserID.ClientID %>").value) > 0) {
            eval(script);
            return true;
        }
        
        window.evalscript = script;
        var msgw, msgh, bordercolor;
        msgw = 400; //提示窗口的宽度 
        msgh = 450; //提示窗口的高度

        var sWidth, sHeight;
        sWidth = document.body.offsetWidth;
        sHeight = document.body.offsetHeight;
        var bgObj = document.createElement("div");
        bgObj.setAttribute('id', 'bgDiv');
        bgObj.style.position = "absolute";
        bgObj.style.top = "0";
        bgObj.style.background = "#777";
        bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
        bgObj.style.opacity = "0.6";
        bgObj.style.left = "0";
        bgObj.style.width = sWidth + "px";
        bgObj.style.height = sHeight + "px";
        bgObj.style.zIndex = "10000";
        document.body.appendChild(bgObj);

        var msgObj = document.createElement("div")
        msgObj.setAttribute("id", "msgDiv");
        msgObj.setAttribute("align", "center");
        msgObj.style.backcolor = "white";
        //msgObj.style.border="1px solid " + bordercolor; 
        msgObj.style.position = "absolute";
        msgObj.style.left = "50%";
        msgObj.style.top = "20%";
        msgObj.style.font = "12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
        msgObj.style.marginLeft = "-225px";
        msgObj.style.marginTop = document.documentElement.scrollTop + "px";
        msgObj.style.width = msgw + "px";
        msgObj.style.height = msgh + "px";
        msgObj.style.textAlign = "center";
        msgObj.style.lineHeight = "25px";
        msgObj.style.zIndex = "10001";

        document.body.appendChild(msgObj);

        var txt = document.createElement("p");
        txt.style.margin = "1em 0"
        txt.setAttribute("id", "msgTxt");

        var s_str = "<table style='border:White 1px solid; background-color:#659FCB;width:400px;' cellspacing=\"0\" cellpadding=\"0\"><tr><td style='padding:5px;'><div style='background-image:url(<%=LoginTopSrcUrl %>); height:55px;'></div><table style='background-image:url(<%=LoginBackSrcUrl %>)' width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" valign=\"middle\"><tr style=\"padding-top: 10px;\"><td width=\"20%\" style=\"height: 24px;\" align=\"right\">用户名：";
        s_str += "</td><td width=\"80%\" style=\"height: 24px\" align=\"left\"><input id=\"tbUserName\" class='in_text' type=\"text\" onkeypress=\"if (window.event.keyCode == 13) {tbPassword.focus();}\" class='in_text' style=\"width:204px;\"/>";
        s_str += "</td></tr><tr><td style=\"height: 29px\"><div align=\"right\">密&nbsp; 码：</div></td><td align=\"left\" style=\"height: 29px\"><input class='in_text' id=\"tbPassWord\" type=\"password\" style=\"width:204px;\" />";
        s_str += "</tr><tr><td height=\"33\" align=\"right\">验证码：</td><td align=\"left\"><table cellspacing=\"0\" cellpadding=\"0\"><tr><td>";
        s_str += "<input name=\"tbCheckCode\" class='in_text' type=\"text\" id=\"tbCheckCode\" style=\"width:64px;\"/></td><td><img id=\"imCheckCode\" name=\"imCheckCode\" style=\"height:24px;width:64px;\"></td><td style='padding-left:10px;'>";
        s_str += "<a href=\"JavaScript:ReloadImage();\" style='color:#0266C5;'>看不清</a></td></tr></table></td></tr><tr><td></td><td height=\"33\" align='left'><table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr style='padding-top:8px;'><td align=\"left\">";
        s_str += "<input type=submit id=imb_Ok name=imb_Ok value=\"登录\" onclick=\"Login()\" style='height:22px;width:58px'></td><td style='padding-left:10px; padding-right:10px;'><input type=button id=imb_Cancel name=imb_Cancel value=\"取消\" onclick=\"Cancel()\" style='height:22px;width:58px'></td><td class='blue'><a href=\"<%=RegisterUrl %>\" target=\"_blank\">免费注册</a><span style='margin-left:10px;'></span><a href=\"<%=ForgetPwdUrl %>\" target=\"_blank\">忘记密码</a></td></tr></table></td></tr>";
        s_str += "</table></td></tr></table>";
        txt.innerHTML = s_str;

        document.getElementById("msgDiv").appendChild(txt);

        var o_CheckCode = document.getElementById("Login_Iframe").contentWindow.document.getElementById("ShoveCheckCode1_imgCode");
        document.getElementById("imCheckCode").src = o_CheckCode.src;
        document.getElementById("imb_Ok").focus();

        return false;
    }
    
    var re;
    function ReloadImage() {        
        window.clearTimeout(re);    
        Login_Iframe.location.href = "<%=LoginIframeUrl %>";
        re = setTimeout("reimage()", 1000);
    }
    
    //等一秒钟再加载
    function reimage() {
        var o_CheckCode = document.getElementById("Login_Iframe").contentWindow.document.getElementById("ShoveCheckCode1_imgCode");
        document.getElementById("imCheckCode").src = o_CheckCode.src;
    }

    function Login() {
        var UserName = document.getElementById("tbUserName").value;
        var PassWord = document.getElementById("tbPassWord").value;
        var CheckCode = document.getElementById("tbCheckCode").value;

        if (UserName == "") {
            alert("用户名不能为空");

            return;
        }

        if (PassWord == "") {
            alert("密码不能为空");

            return;
        }

        if (CheckCode == "") {
            alert("验证码不能为空");

            return;
        }

        var inputCheckCode = document.getElementById("Login_Iframe").contentWindow.document.getElementById("ShoveCheckCode1_tbCode").value;

        var Result = Buy_Luck_Buy.Login(UserName, PassWord, CheckCode, inputCheckCode, 1).value;

        if (Result == null || isNaN(Result)) {
            Result = Buy_Luck_Buy.Login(UserName, PassWord, CheckCode, inputCheckCode, 1).value;
        }

        if (Result == null) {
            alert("登录异常，请重试一次，谢谢。可能是网络延时原因。");

            return;
        }

        if (isNaN(Result)) {
            alert(Result);

            return;
        }

        document.getElementById("<%=HidUserID.ClientID %>").value = Result;

        Cancel();
        
        location.href = location.href;

        if (window.evalscript != "") {
            eval(window.evalscript);
        }
    }

    function Cancel() {
        document.body.removeChild(document.getElementById("bgDiv"));
        document.body.removeChild(document.getElementById("msgDiv"));
        return false;
    }

</script>
    </form>
</body>
</html>
