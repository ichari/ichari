<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BaseInfo.aspx.cs" Inherits="Cps_Administrator_BaseInfo" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <link href="../../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../../Style/main.css" type="text/css" rel="stylesheet" />

    <script language="javascript" type="text/javascript">
        function Clear()
        {
            var controls = document.all.tags("input");
            
            for(var i=0;i<controls.length;i++)
            {
                if(controls[i].type=="text")
                {
                    controls[i].value="";
                }
            }
        }
        function isNull()
        {
            if(document.getElementById('tbDomainName').value == "")
            {
                alert("请输入推广地址!");
                document.getElementById('tbDomainName').focus();
                
                return false;
            }
            
             if(document.getElementById('tbUrlName').value=="")
            {
                alert("请输入网站名称!");
                document.getElementById('tbUrlName').focus();
                return false;
            }
             if(document.getElementById('tbUrl').value=="")
            {
                alert("请输入网址!");
                document.getElementById('tbUrl').focus();
                return false;
            }
           
             if(document.getElementById('tbMobile').value == "")
            {
                alert("请输入手机号码!");
                document.getElementById('tbMobile').focus();
                return false;
            }
            
            if(document.getElementById('tbEmail').value == "")
            {
                alert("请输入Email!");
                document.getElementById('tbEmail').focus();
                
                return false;
            }
        }
    </script>



</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td height="30" background="images/bg_hui_33.gif" bgcolor="#F5f5f5" class="black14"
                style="padding-left: 20px;">
                <asp:Label ID="lbCpsType" runat="server">  CPS 代理商管理</asp:Label>
                &gt; 代理商资料
            </td>
        </tr>
    </table>
    <div style="padding-top: 10px;">
    </div>
    <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
        <tr>
            <td height="30" align="center" bgcolor="#E9F1F8" class="blue14" style="padding-left: 10px;
                padding-right: 10px;">
                <asp:Label ID="lbShow" runat="server" Text="商家资料"></asp:Label>
            </td>
        </tr>
        <tr>
            <td height="30" align="left" bgcolor="#FFFFFF" style="padding: 10px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="red14">
                            推广信息：
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr>
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        CPS 模式：
                                    </td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 2px;">
                                        &nbsp;&nbsp;<asp:Label ID="lbType" runat="server" Text="推广员" CssClass="black12"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        推广地址：
                                    </td>
                                    <td bgcolor="#FFFFFF" class="blue" style="padding-left: 10px;">
                                       
                                        <asp:TextBox ID="tbDomainName" runat="server"  Enabled="false"
                                            autocomplete="off" Width="650px" Height="18px" ForeColor="#CC0000"/>
                                    </td>
                                </tr>
                                <tr id="trMD5Key" runat="server" visible="false">
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        MD5校验码：</td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <asp:TextBox ID="tbMD5Key" CssClass="in_text_hui" runat="server"
                                            autocomplete="off" MaxLength="64"  />
                                        <span class="red142"></span>
                                    </td>
                                </tr>
                                  <tr>
                                    <td height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        返点比例：
                                    </td>
                                    <td bgcolor="#FFFFFF" class="blue" style="padding-left: 10px;">
                                       <asp:HyperLink ID="linkBonusScale" runat="server" Target="_self" 
                                            Font-Underline="True" ForeColor="#CC0000">查看/设置返点比例</asp:HyperLink>
                                  </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td height="30" class="red14">
                            联系人以及联系方式：
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr>
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        网站名称：
                                    </td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <asp:TextBox ID="tbUrlName" CssClass="in_text_hui" runat="server" autocomplete="off" />
                                        <span  style="color:Red">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        网址：
                                    </td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <asp:TextBox ID="tbUrl" CssClass="in_text_hui" runat="server" autocomplete="off" />
                                        <span  style="color:Red">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" align="right" bgcolor="#F7F7F7" class="black12">
                                        联系人：</td>
                                        <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                            <asp:TextBox ID="tbContactPerson" CssClass="in_text_hui" runat="server" autocomplete="off" />
                                            <span  style="color:Red">*</span>
                                        </td>
                                </tr>
                                <tr>
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        联系电话：
                                    </td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <asp:TextBox ID="tbPhone" CssClass="in_text_hui" runat="server" autocomplete="off"
                                            onkeyup="value=value.replace(/[^\d\-]/g,'');" />
                                        <span  style="color:Red"> 例如：0755-61159086</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        手机号码：
                                    </td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <asp:TextBox ID="tbMobile" CssClass="in_text_hui" runat="server" autocomplete="off"
                                            onkeyup="value=value.replace(/[^\d]/g,'');" />
                                        <span  style="color:Red">* </span>
                             
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        QQ 号码：
                                    </td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <asp:TextBox ID="tbQQNum" CssClass="in_text_hui" runat="server" autocomplete="off"
                                            onkeyup="value=value.replace(/[^\d]/g,'');" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="12%" height="28" align="right" bgcolor="#F7F7F7" class="black12">
                                        Email：
                                    </td>
                                    <td bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <asp:TextBox ID="tbEmail" CssClass="in_text_hui" runat="server" autocomplete="off" /><span  style="color:Red">*</span>
                                       
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnOK" runat="server" Text="确 定" OnClick="btnOK_Click" OnClientClick="return isNull();" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnClear" runat="server" Text="重 填" OnClientClick="return Clear();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hType" runat="server" />
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
