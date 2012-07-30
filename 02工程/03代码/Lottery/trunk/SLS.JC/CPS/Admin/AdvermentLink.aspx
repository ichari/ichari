<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdvermentLink.aspx.cs" Inherits="CPS_Admin_AdvermentLink" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="770" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr class="table">
                                    <td height="33" background="../images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="hui">
                                                    推广首页&nbsp;&gt;推广中心&nbsp;&gt;&nbsp;广告链接
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px;">
                            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                <tr>
                                    <td height="30" colspan="2" align="center" bgcolor="#EAF2FE">
                                        <strong class="blue12">广告链接</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                        <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ccccccc">
                                            <tr>
                                                <td width="23%" height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    链接地址：
                                                </td>
                                                <td width="77%" height="28" align="left" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <span class="red14" id="spanLinkUrl" runat="server"></span><input type="button" value="复制" onclick="doufucopy('spanLinkUrl')"/>
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    CPS模式：
                                                </td>
                                                <td height="28" align="left" id="tdType" runat="server" bgcolor="#FFFFFF" class="blue" style="padding-left: 10px;">
                                                    推广员
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    返点比例：
                                                </td>
                                                <td height="28" align="left" bgcolor="#FFFFFF" class="red" style="padding-left: 10px;">
                                                    <a href="BonusScaleView.aspx">查看返点比例<span class="hui_2"></span></a>
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td height="40" colspan="2" align="center" bgcolor="#FFFFFF" class="blue">
                                                    <input  type="button" onclick="window.open('QueryBonusFlow.html')" value="查看CPS计算方式" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="98%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                                            <tr>
                                                <td class="red14">
                                                    商家资料信息：
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#CCCCCC">
                                            <tr>
                                                <td width="13%" height="30" align="right" bgcolor="#F5F5F5">
                                                    真实姓名：
                                                </td>
                                                <td width="39%" bgcolor="#FFFFFF" style="padding-left: 10px;" id="tdRealyName" runat="server">
                                                    
                                                </td>
                                                <td width="17%" align="right" bgcolor="#F5F5F5">
                                                    ID：
                                                </td>
                                                <td width="31%" bgcolor="#FFFFFF" style="padding-left: 10px;" id="tdUserName" runat="server">
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="30" align="right" bgcolor="#F5F5F5">
                                                    联系人：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <asp:TextBox ID="tbContactPerson" size="30" runat="server" autocomplete="off" /><span class="red">*</span>
                                                </td>
                                                <td align="right" bgcolor="#F5F5F5">
                                                    联系电话：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <asp:TextBox ID="tbPhone"  size="30" runat="server" autocomplete="off" onkeyup="value=value.replace(/[^\d\-]/g,'');" /><span class="red">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="30" align="right" bgcolor="#F5F5F5">
                                                    手机号码：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                   <asp:TextBox ID="tbMobile" size="30" runat="server" autocomplete="off" onkeyup="value=value.replace(/[^\d]/g,'');" /><span class="red">*</span>
                                                </td>
                                                <td align="right" bgcolor="#F5F5F5" style="padding-left: 10px;">
                                                    MSN,QQ：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <asp:TextBox ID="tbQQNum" size="30" runat="server" autocomplete="off" onkeyup="value=value.replace(/[^\d]/g,'');" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="30" align="right" bgcolor="#F5F5F5">
                                                    Email：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <asp:TextBox ID="tbEmail" size="30"  runat="server" autocomplete="off" /><span class="red">*</span>
                                                </td>
                                                <td align="right" bgcolor="#F5F5F5">
                                                    网站名称：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <asp:TextBox ID="tbUrlName" size="30" runat="server" autocomplete="off" /><span class="red">*</span>
                                                </td>
                                            </tr>
                                            <tr >
                                                <td height="30" align="right" bgcolor="#F5F5F5">
                                                    网址：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;" id="tdUrl" runat="server">
                                                    <asp:TextBox ID="tbUrl"  size="30" runat="server" autocomplete="off" /><span class="red">*</span>
                                                </td>
                                                <td bgcolor="#F5F5F5" height="30" align="right" id="tdMD51" runat="server" visible="false">
                                                    MD5校验码：
                                                </td>
                                                <td bgcolor="#FFFFFF" style="padding-left: 10px;" id="tdMD52" runat="server" visible="false">
                                                    <asp:TextBox ID="tbMD5Key" size="30" runat="server" autocomplete="off" MaxLength="64"  /><span class="red">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="40" align="center" colspan="4" bgcolor="#FFFFFF">
                                                    <asp:Button ID="btnOK" runat="server" Text="确 定" 
                                                        OnClientClick="return isNull();" onclick="btnOK_Click" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<input type="button" value="重 填" onclick="clears();" />
                                                </td>  
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hidType" runat="server" Value="1" />
    </form>
</body>
</html>
<script type="text/javascript">
    function clears() {
        var controls = document.getElementsByTagName("input");

        for (var i = 0; i < controls.length; i++) {
            if (controls[i].type == "text") {
                controls[i].value = "";
            }
        }
    }

    function isNull() {
        if (document.getElementById('tbUrlName').value == "") {
            alert("请输入网站名称!");
            document.getElementById('tbUrlName').focus();
            return false;
        }

        if (document.getElementById('tbUrl').value == "") {
            alert("请输入网址!");
            document.getElementById('tbUrl').focus();
            return false;
        }
        
        if (document.getElementById("hidType").value == "2") {
            if (document.getElementById('tbMD5Key').value == "") {
                alert("请输入MD5校验码!");
                document.getElementById('tbMD5Key').focus();
                return false;
            }
        }
        
        if (document.getElementById('tbContactPerson').value == "") {
            alert("请输入联系人!");
            document.getElementById('tbContactPerson').focus();

            return false;
        }

        if (document.getElementById('tbPhone').value == "") {
            alert("请输入联系电话!");
            document.getElementById('tbPhone').focus();

            return false;
        }
        if (document.getElementById('tbMobile').value == "") {
            alert("请输入手机号码!");
            document.getElementById('tbMobile').focus();
            return false;
        }

        if (document.getElementById('tbEmail').value == "") {
            alert("请输入Email!");
            document.getElementById('tbEmail').focus();

            return false;
        }
    }

    function doufucopy(span) {
        clipboardData.setData("text", document.getElementById(span).innerText);
        alert("复制成功！");
    }
</script>