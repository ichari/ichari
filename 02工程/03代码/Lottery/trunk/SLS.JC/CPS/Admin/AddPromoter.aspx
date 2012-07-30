<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPromoter.aspx.cs" Inherits="CPS_Admin_AddPromoter" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="770" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr class="table">
                                    <td height="33" background="../images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="hui">
                                                    推广首页&nbsp;&gt;推广员管理&nbsp;&gt;&nbsp;增加推广员
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
                                        <strong class="blue12">增加推广员</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                         <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            用户名：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 2px;" class="blue">
                                            &nbsp;
                                            <asp:TextBox ID="tbUserName" runat="server" CssClass="in_text_hui" />
                                             &nbsp;<span class="red142">*</span><span style="color: Red;">用户名长度在 5-16 个英文字符或数字、中文 
                                            3-8 之间。</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            密码：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 2px;" class="blue">
                                            &nbsp;
                                            <asp:TextBox ID="tbPassword" runat="server" CssClass="in_text_hui"  TextMode="Password"/>
                                             &nbsp;<span class="red142">*</span><span style="color: Red;">密码长度必须在 6-16 位之间。</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                            确认密码：
                                        </td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 2px;" class="blue">
                                            &nbsp;
                                            <asp:TextBox ID="tbPwd" runat="server" CssClass="in_text_hui"  TextMode="Password"/>
                                            &nbsp;<span class="red142">*</span><span style="color: Red;">密码长度必须在 6-16 位之间。</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" bgcolor="#F7F7F7" class="black12" height="28" width="12%">
                                            网站名称： 
                                        </td>
                                        <td bgcolor="#FFFFFF" class="blue" style="padding-left: 2px;">
                                            &nbsp;
                                            <asp:TextBox ID="tbUrlName" runat="server" autocomplete="off" 
                                                CssClass="in_text_hui" />
                                            <span class="red142">&nbsp;*</span>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="right" bgcolor="#F7F7F7" class="black12" height="28" width="12%">
                                            网址： 
                                        </td>
                                        <td bgcolor="#FFFFFF" class="blue" style="padding-left: 2px;">
                                           &nbsp;
                                            <asp:TextBox ID="tbUrl" runat="server" autocomplete="off" 
                                                CssClass="in_text_hui" />
                                            <span class="red142">&nbsp;*</span>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="right" bgcolor="#F7F7F7" class="black12" height="28" width="12%">
                                            联系人：</td>
                                        <td bgcolor="#FFFFFF" class="blue" style="padding-left: 2px;">
                                            &nbsp;
                                            <asp:TextBox ID="tbContactPerson" runat="server" autocomplete="off" 
                                                CssClass="in_text_hui" />
                                            <span class="red142">&nbsp;*</span>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="right" bgcolor="#F7F7F7" class="black12" height="28" width="12%">
                                            联系电话： 
                                        </td>
                                        <td bgcolor="#FFFFFF" class="blue" style="padding-left: 2px;">
                                            &nbsp;
                                            <asp:TextBox ID="tbPhone" runat="server" autocomplete="off" 
                                                CssClass="in_text_hui" onkeyup="value=value.replace(/[^\d\-]/g,'');" />
                                            <span class="red142">&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" bgcolor="#F7F7F7" class="black12" height="28" width="12%">
                                            手机号码： 
                                        </td>
                                        <td bgcolor="#FFFFFF" class="blue" style="padding-left: 2px;">
                                            &nbsp;
                                            <asp:TextBox ID="tbMobile" runat="server" autocomplete="off" 
                                                CssClass="in_text_hui" onkeyup="value=value.replace(/[^\d]/g,'');" />
                                            <span class="red142">&nbsp;* </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" bgcolor="#F7F7F7" class="black12" height="28" width="12%">
                                            QQ 号码： 
                                        </td>
                                        <td bgcolor="#FFFFFF" class="blue" style="padding-left: 2px;">
                                            &nbsp;
                                            <asp:TextBox ID="tbQQNum" runat="server" autocomplete="off" 
                                                CssClass="in_text_hui" onkeyup="value=value.replace(/[^\d]/g,'');" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" bgcolor="#F7F7F7" class="black12" height="28" width="12%">
                                            Email： 
                                        </td>
                                        <td bgcolor="#FFFFFF" class="blue" style="padding-left: 2px;">
                                            &nbsp;
                                            <asp:TextBox ID="tbEmail" runat="server" autocomplete="off" 
                                                CssClass="in_text_hui" />
                                            <span class="red142">&nbsp;*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" bgcolor="#F7F7F7" class="black12" height="28" colspan="2">
                                            <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" 
                                    OnClientClick="return isNull();" Text="确 定" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnClear" runat="server" OnClientClick="return Clear();" 
                                    Text="重 填" />
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
    </form>
</body>
</html>
 <script language="javascript" type="text/javascript">
        function Clear() {
            var controls = document.getElementsByTagName("input");

            for (var i = 0; i < controls.length; i++) {
                if (controls[i].type == "text") {
                    controls[i].value = "";
                }
            }

            return false;
        }

        function isNull() {
            var name = document.getElementById('tbUserName');

            if (name.value == "" || name.value.length < 5 || name.value.length > 16) {
                alert("用户名长度在 5-16 个英文字符或数字、中文 3-8 之间！");
                name.focus();

                return false;
            }

            var password = document.getElementById('tbPassword');

            if (password.value == "" || password.value.length < 6 || password.value.length > 16) {
                alert("密码长度必须在 6-16 位之间！");
                password.focus();

                return false;
            }

            var pwd = document.getElementById('tbPwd');

            if (pwd.value != password.value) {
                alert("两次密码输入不一致，请仔细检查！");
                pwd.focus();

                return false;
            }

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
            if (document.getElementById('tbContactPerson').value == "") {
                alert("请输入联系人!");
                document.getElementById('tbContactPerson').focus();

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
       
    </script>