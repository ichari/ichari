<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FollowFriendSchemeAdd.aspx.cs" Inherits="Home_Room_FollowFriendSchemeAdd" %>
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
     
    <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="../../JScript/Public.js"></script>
    <link href="/Style/pub.css" rel="stylesheet" type="text/css" />
    <link href="/Style/thickbox.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function ShowOrHideDZUserList()
        {
            if(document.all["FollowUseList"].style.display == "")
            {
                document.all["FollowUseList"].style.display = "none";
            }
            else
            {
                document.all["FollowUseList"].style.display = "";
            }
        }

        function InputMask_Number()
        {
	        if (window.event.keyCode < 48 || window.event.keyCode > 57)
		        return false;
	        return true;
        }

        function CheckMultiple(sender)
        {
            var multiple = StrToInt(sender.value);
            
            sender.value = multiple;
            
            if (multiple < 1)
            {
	            if (confirm("输入不正确，按“确定”重新输入，按“取消”自动更正为 1 ，请选择。"))
	            {
		            sender.focus();
		            return;
	            }
	            else
	            {
		            multiple = 1;
		            sender.value = 1;
	            }
            }
        }
    </script>

    <base target="_self" />
    </head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#9FC8EA">
            <tr>
                <td valign="top" bgcolor="#FFFFFF" >
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#F4F9FC" style="border-left: 1px solid #DDDDDD;
                        border-right: 1px solid #DDDDDD;" >
                        <tr >
                            <td height="29">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border-right-color:#D8D8D8; border-left-color:#D8D8D8;">
                                    <tr>
                                        <td width="40" height="29" align="center">
                                            <img src="images/jiantou_5.gif" width="12" height="8" />
                                        </td>
                                        <td class="blue_num" >
                                            定制自动跟单
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <table width="100%" align="center" cellpadding="5" cellspacing="1" bgcolor="#D8D8D8" >
                            <tr>
                                <td width="130" bgcolor="#f5f5f5" style="text-align: right; height:25px">
                                    好友用户名
                                </td>
                                <td bgcolor="#FFFFFF">
                                    <asp:Label ID="lbUserName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#f5f5f5" style="text-align: right;">
                                    定制自动跟单玩法
                                </td>
                                <td bgcolor="#FFFFFF">
                                    <asp:DropDownList ID="ddlLotteries" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLotteries_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlPlayTypes" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#f5f5f5" style="text-align: right;">
                                    每次跟单金额
                                </td>
                                <td bgcolor="#FFFFFF">
                                                <asp:TextBox ID="tbMinMoney" runat="server" Width="50px" onkeypress="return InputMask_Number();"
                                                    onblur="CheckMultiple(this);" Text="1"></asp:TextBox> 至 <asp:TextBox ID="tbMaxMoney" Text="1"
                                                        runat="server" Width="50px" onkeypress="return InputMask_Number();" onblur="CheckMultiple(this);"></asp:TextBox>&nbsp;元
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#f5f5f5" style="text-align: right;">
                                    每次认购份数
                                </td>
                                <td bgcolor="#FFFFFF">
                                    <asp:TextBox ID="tbBuyShareStart" runat="server" Width="50px" onkeypress="return InputMask_Number();"
                                        onblur="CheckMultiple(this);" Text="1"></asp:TextBox> 至 
                                        <asp:TextBox ID="tbBuyShareEnd" runat="server" Width="50px" onkeypress="return InputMask_Number();" Text="1"
                                        onblur="CheckMultiple(this);"></asp:TextBox>&nbsp;份
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#f5f5f5">
                                </td>
                                <td bgcolor="#FFFFFF">
                                    <asp:Button  ID="btn_OK" runat="server" Text=" 确定 " OnClientClick="return createUserLogin()"  OnClick="btn_OK_Click" />
                                    <span style="margin-left: 100px;"></span>
                                    <asp:Button ID="btnCancel" runat="server" Text=" 取消 " 
                                        onclick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                        
                    </div>
                </td>
            </tr>
        </table>
        <br />
           <table width="100%" border="0" cellspacing="1" cellpadding="0" 
                    bgcolor="#BCD2E9">
                    <tr>
                        <td height="28" colspan="6" bgcolor="#E7F1FA" class="blue12_1" style="padding-left: 15px;">
                            <strong>总跟单人次排行TOP10</strong>
                        </td>
                    </tr>
                    <tr>
                        <td width="8%" height="28" align="center" bgcolor="#F4F9FC" class="blue12">
                            排名
                        </td>
                        <td width="30%" height="28" align="center" bgcolor="#F4F9FC" class="blue12">
                            发起人
                        </td>
                        <td width="11%"  align="center" bgcolor="#F4F9FC" class="blue12">
                            被定制人气
                        </td>
                        <td width="22%"  align="center" bgcolor="#F4F9FC" class="blue12">
                            跟单总金额
                        </td>
                        <td width="20%"  align="center" bgcolor="#F4F9FC" class="blue12">
                            当前期方案
                        </td>
                        <td width="9%" align="center" bgcolor="#F4F9FC" class="blue12">
                            自动跟单
                        </td>
                    </tr>
                    <tbody id="tbFollowList" runat="server">
       
                    </tbody>
                </table>
    </div>
    <asp:HiddenField ID="HidFollowUserID" runat="server" />
    <asp:HiddenField ID="HidNumber" runat="server" /> <!--未登录提示层-->
<div style="display: none; width: 360px;" id="loginLay">
    <div>
        <div class="tips_text">
            <div class="dl_tips" id="error_tips" style="display: none;">
                <b class="dl_err"></b>您输入的账户名和密码不匹配，请重新输入。</div>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="dl_tbl">
                <tr>
                    <td style="width: 70px;">
                        用户名：
                    </td>
                    <td>
                        <input type="text" class="tips_txt" id="lu" name="lu" />
                    </td>
                    <td class="t_ar">
                        <a href="/UserReg.aspx" target="_blank" tabindex="-1">免费注册</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        密&nbsp;&nbsp;码：
                    </td>
                    <td>
                        <input type="password" class="tips_txt" id="lp" name="lp" />
                    </td>
                    <td class="t_ar">
                        <a href="/ForgetPassword.aspx" target="_blank" tabindex="-1">忘记密码</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        验证码：
                    </td>
                    <td colspan="2">
                        <input type="text" class="tips_yzm" id="yzmtext" name="c" /><img alt="验证码" src="about:blank"
                            id="yzmimg" style="cursor: pointer; width: 60px; height: 25px"><a class="kbq" href="#"
                                id="yzmup">看不清，换一张</a>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <input type="button" class="btn_Dora_b" value="登 录" id="floginbtn" style="margin-right: 18px" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div id="DivUserinfo">
    <input id="head_HidUserID" name="head_HidUserID" value="-1" type="hidden"/>
</div>
<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script src="/JScript/thickbox.js" type="text/javascript"></script>
<script src="/JScript/head.js" type="text/javascript"></script>
    </form>
</body>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</html>
<script type="text/javascript" language="javascript">
    parent.document.getElementById('iframeFollowScheme').style.height = parent.iframeFollowScheme.document.body.scrollHeight;

    function createUserLogin() {
        if (CreateLogin('')) {
            if (confirm("您确认输入无误并定制跟单吗?")) {
                return true;
            }
            return false;
        }
        return false;
    }
    </script>
