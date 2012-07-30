<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserRegCPS.aspx.cs" Inherits="CPS_UserRegCPS" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<%@ Register Src="UserControls/Head.ascx" TagName="Head" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>兼职推广联盟</title>
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Head ID="Head1" runat="server" />
    <table width="981" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;"
        align="center">
        <tr>
            <td width="220" valign="top">
                <table width="220" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                    <tr>
                        <td height="32" background="images/index-1_28.gif" bgcolor="#FFFFFF" class="blue12"
                            style="padding-left: 10px;">
                            <strong>申请推广</strong>
                        </td>
                    </tr>
                    <tr>
                        <td height="70" background="images/index-1_32.gif" bgcolor="#FFFFFF">
                            <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="10%" align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td width="90%" height="26" class="hui">
                                        <a href="UserRegCPS.aspx">注册推广员</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="UserRegCPS.aspx?type=2">注册代理商</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="Login.aspx">推广员登录</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <img src="images/dian.jpg" width="3" height="3" />
                                    </td>
                                    <td height="26" class="hui">
                                        <a href="../Home/Web/UserRegAgree.aspx?type=1" target="_blank">注册协议</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="558" rowspan="2" align="left" valign="top">
                <table width="550" border="0" align="left" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC"
                    style="margin-left: 4px;">
                    <tr>
                        <td height="33" background="images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                            <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td class="hui">
                                        推广首页&nbsp;&gt;&nbsp;申请推广&nbsp;&gt;&nbsp;<span id="spanType" runat="server">注册推广员</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" bgcolor="#FFFFFF" style="padding-top: 8px; padding-bottom: 8px;"
                            id="tdCpsApply" runat="server">
                            <table width="540" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                </tr>
                                <tr>
                                    <td align="center" class="cheng12">
                                        <table width="95%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" style="text-indent: 30px;">
                                                    <span id="spanCommender">推广员，是指一定流量的个人站长，在线申请，无需审核，直接开通，可获得2%的交易量返点。</span> <span
                                                        id="spanAgent" style="display: none">代理商，是指具有大量下属站长资源的广告站长联盟，在线申请，审核通过获得CPS广告代理权，获得超过2.0%的交易返点。</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="hui" style="padding-top: 10px;">
                                        <table width="510" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="77" align="right">
                                                    <span class="hui">CPS模式</span>：
                                                </td>
                                                <td height="30" colspan="2">
                                                    <asp:DropDownList ID="ddlCpsType" runat="server" onchange="SelectChange()">
                                                        <asp:ListItem Value="1">推广员</asp:ListItem>
                                                        <asp:ListItem Value="2">代理商</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <span class="red">*</span>
                                                </td>
                                            </tr>
                                            <tbody id="tbUser" runat="server">
                                                <tr>
                                                    <td align="right" class="hui">
                                                        你的用户名：
                                                    </td>
                                                    <td width="241">
                                                        <asp:TextBox ID="tbUserName" runat="server" size="30" MaxLength="16"></asp:TextBox>
                                                        <span class="red">*</span>
                                                    </td>
                                                    <td width="192" height="30">
                                                        <input id="btnCheckUserName" type="button" value="检测用户名是否可用" onclick="return checkUserName();" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="30" align="right">
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <span class="red" id="spCheckResult">用户名长度在 5-16 个英文字符或数字、中文 3-8 之间。</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="hui">
                                                        你的密码：
                                                    </td>
                                                    <td height="30" colspan="2">
                                                        <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" Width="209" MaxLength="16"></asp:TextBox>
                                                        <span class="red">*</span> <span class="hui_2">你的密码长度在6—16位之间</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="hui">
                                                        真实姓名：
                                                    </td>
                                                    <td height="30" colspan="2">
                                                        <asp:TextBox ID="tbRealyName" runat="server" size="30"></asp:TextBox>
                                                        <span class="red">*</span> <span class="hui_2">非常重要，这是您提款的重要依据，必须与提款银行卡户名一致。真实姓名一旦提交将不可更改</span>
                                                    </td>
                                                </tr>
                                            </tbody>
                                                <tr>
                                                    <td align="right" class="hui">
                                                        网站名称：
                                                    </td>
                                                    <td height="30" colspan="2">
                                                        <asp:TextBox ID="tbSiteName" runat="server" size="30"></asp:TextBox>
                                                        <span class="red">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="hui">
                                                        网址：
                                                    </td>
                                                    <td height="30" colspan="2">
                                                        <asp:TextBox ID="tbWebUrl" runat="server" size="30"></asp:TextBox>
                                                        <span class="red">*</span>
                                                    </td>
                                                </tr>
                                                <tr style="display: none" id="trMD5">
                                                    <td align="right" class="hui">
                                                        MD5校验码：
                                                    </td>
                                                    <td height="30" colspan="2">
                                                        <asp:TextBox ID="tbMD5" runat="server" size="30"></asp:TextBox>
                                                        <span class="red">*</span> <span class="hui_2">调用接口的密钥</span>
                                                    </td>
                                                </tr>
                                            <tr>
                                                <td align="right" class="hui">
                                                    手机号码：
                                                </td>
                                                <td height="30" colspan="2">
                                                    <asp:TextBox ID="tbPhone" runat="server" size="30"></asp:TextBox>
                                                    <span class="red">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="hui">
                                                    电子邮箱：
                                                </td>
                                                <td height="30" colspan="2">
                                                    <asp:TextBox ID="tbEmail" runat="server" size="30"></asp:TextBox>
                                                    <span class="red">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="hui">
                                                    QQ：
                                                </td>
                                                <td height="30" colspan="2">
                                                    <asp:TextBox ID="tbQQ" runat="server" size="30" MaxLength="16"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="CheckCode" runat="server">
                                                <td align="right" class="hui">
                                                    验证码：
                                                </td>
                                                <td height="30" colspan="2">
                                                    <asp:TextBox ID="tbCheckCode" runat="server" MaxLength="6" Width="50px"></asp:TextBox>
                                                    <ShoveWebUI:ShoveCheckCode ID="ShoveCheckCode1" runat="server" ForeColor="CornflowerBlue"
                                                        BackColor="SeaShell" Charset="All" Height="20px" SupportDir="~/ShoveWebUI_client"
                                                        Style="vertical-align: top; padding-top: 3px;"></ShoveWebUI:ShoveCheckCode>
                                                    <span class="red">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="hui">
                                                    &nbsp;
                                                </td>
                                                <td height="15" colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:CheckBox ID="cbAgree" runat="server" Checked="true" />
                                                </td>
                                                <td height="30" colspan="2">
                                                    同意广告商家<a href="../Home/Web/UserRegAgree.aspx?type=1" target="_blank"><span class="red">注册协议</span></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <table width="300" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <ShoveWebUI:ShoveConfirmButton ID="btnReg" runat="server" BackgroupImage="images/bt_zhuce.jpg"
                                                                    BorderStyle="None" CausesValidation="False" Height="29px" OnClientClick="if(!checkReg()){return false};"
                                                                    Style="cursor: pointer;" Width="83px" OnClick="btnReg_Click" />
                                                            </td>
                                                            <td align="right">
                                                                <img src="images/bt_chongtian.jpg" width="83" height="29" border="0" onclick="clears();"
                                                                    style="cursor: pointer" />
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
                    </tr>
                </table>
            </td>
            <td width="203" rowspan="2" align="left" valign="top">
                <table width="196" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="196" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="30" background="images/index-1_28.gif" class="blue12" style="padding-left: 10px;">
                                        <strong>推广员佣金排行</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#FFFFFF" style="padding-top: 10px; padding-bottom: 10px;" id="tdUsers"
                                        runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-top: 6px;">
                            <table width="196" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="30" align="left" bgcolor="#ECECEC" class="hui" style="padding-left: 10px;">
                                        <strong>推广指南</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#FFFFFF" id="tdNews" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px;">
                &nbsp;
            </td>
        </tr>
    </table>
    <uc2:Foot ID="Foot1" runat="server" />
    <asp:HiddenField ID="hUserID" runat="server" Value="-1" />
    <asp:HiddenField ID="hType" runat="server" Value="1" />
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>

<script type="text/javascript" language="javascript">
        function checkUserName() {

            var userName = document.getElementById("tbUserName").value;
            var result = CPS_UserRegCPS.CheckUserName(userName).value;

            if (userName.trim() == "") {
                spCheckResult.innerHTML = "用户名不能为空";
                document.getElementById("tbUserName").value = "";
                alert(spCheckResult.innerHTML);
                return false;
            }

            if (Number(result) < 0) {
                if (Number(result) == -1) {
                    spCheckResult.innerHTML = "<font color='red'>对不起用户名中含有禁止使用的字符</font>";
                    alert("对不起用户名中含有禁止使用的字符");
                    return false;
                }

                if (Number(result) == -2) {
                    spCheckResult.innerHTML = "<font color='red'>用户名 " + userName + " 已被占用，请重新输入一个</font>";
                    alert("用户名 " + userName + " 已被占用，请重新输入一个");
                    return false;
                }

                if (Number(result) == -3) {
                    spCheckResult.innerHTML = "<font color='red'>用户名长度在 5-16 个英文字符或数字、中文 3-8 之间。</font>";
                    alert("用户名长度在 5-16 个英文字符或数字、中文 3-8 之间。");
                    return false;
                }
            }
            else {
                spCheckResult.innerHTML = "<font color='green'>用户名 <font color='red'>" + userName + "</font> 可以使用</font>";

                return true;
            }
        }

        function checkReg() {
            var realyName = "";
            var password = "";
            var md5 = "";
            var userName = "";
            var phone = document.getElementById('tbPhone').value;
            var email = document.getElementById('tbEmail').value;
            var siteName = document.getElementById('tbSiteName').value;
            var webUrl = document.getElementById('tbWebUrl').value;
           
            
            var userid = parseInt(document.getElementById("hUserID").value);
            if(userid < 1)
            {
            userName = document.getElementById("tbUserName").value;
            password = document.getElementById("tbPassword").value;
            realyName = document.getElementById("tbRealyName").value;
            if (userName.trim() == "") {
                document.getElementById("tbUserName").value = "";
                alert("用户名不能为空。");
                document.getElementById('tbUserName').focus();

                return false;
            }

            if (password.trim() == "") {
                document.getElementById("tbPassword").value = "";
                alert("密码不能为空。");
                document.getElementById('tbPassword').focus();

                return false;
            }
            
             if (realyName.trim() == "") {
                document.getElementById("tbRealyName").value = "";
                alert("真实姓名不能为空。");
                document.getElementById('tbRealyName').focus();

                return false;
            }
            }

            if (siteName.trim() == "") {
                document.getElementById('tbSiteName').value = "";
                alert("请输入网站名称!");
                document.getElementById('tbSiteName').focus();
                return false;
            }
            if (webUrl.trim() == "") {
                document.getElementById('tbWebUrl').value = "";
                alert("请输入网址!");
                document.getElementById('tbWebUrl').focus();
                return false;
            }
            
            if (document.getElementById("trMD5").style.display != "none") {
                 
                 md5 = document.getElementById('tbMD5').value;
                
                if (md5.trim() == "") {
                    document.getElementById('tbMD5').value = "";
                    alert("请输入校验码!");
                    document.getElementById('tbMD5').focus();
                    return false;
                }
            }

            if (phone.trim() == "") {
                document.getElementById('tbPhone').value = "";
                alert("请输入联系电话!");
                document.getElementById('tbPhone').focus();
                return false;
            }

            if (email.trim() == "") {
                document.getElementById('tbEmail').value = "";
                alert("请输入电子信箱！");
                document.getElementById('tbEmail').focus();
                return false;
            }

            if (!document.getElementById("cbAgree").checked) {
                alert("必须同意注册协议！");

                return false;
            }
           
            if (userid < 1 && !checkUserName()) {
                return false;
            }

            var result = CPS_UserRegCPS.CheckReginfo(userName, password, realyName, siteName, webUrl, phone, email, userid.toString()).value;
           
            if (result != "") {
                alert(result);
                return false;
            }

            return true;
        }

        function SelectChange() {
            var ddlChange = document.getElementById("ddlCpsType");
            if (ddlChange.options[ddlChange.selectedIndex].value == "1") {
                document.getElementById("spanType").innerHTML = "注册推广员";
                document.getElementById("spanCommender").style.display = "";
                document.getElementById("spanAgent").style.display = "none";
                document.getElementById("trMD5").style.display = "none";
            }
            else {
                document.getElementById("spanType").innerHTML = "注册代理商";
                document.getElementById("spanCommender").style.display = "none";
                document.getElementById("spanAgent").style.display = "";
                document.getElementById("trMD5").style.display = "";
            }
        }

        if (document.getElementById("hType").value == "2") {
            if (document.getElementById("ddlCpsType")!=null) {
                document.getElementById("ddlCpsType").value = "2";
            }
            if (document.getElementById("spanCommender") != null) {
                document.getElementById("spanCommender").style.display = "none";
            }
            if (document.getElementById("spanAgent") != null) {
                document.getElementById("spanAgent").style.display = "";
            }
            if (document.getElementById("trMD5") != null) {
                document.getElementById("trMD5").style.display = "";
            }
        }
        
        function clears()
        {
            var controls = document.getElementsByTagName("input");
            
            for(var i=0;i<controls.length;i++)
            {
                if(controls[i].type == "text")
                {
                    controls[i].value = "";
                }
            }
        }
</script>

