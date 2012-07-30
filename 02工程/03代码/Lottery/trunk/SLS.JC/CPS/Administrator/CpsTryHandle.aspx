<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CpsTryHandle.aspx.cs" Inherits="Cps_Administrator_CpsTryHandle"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../../Style/css.css" type="text/css" rel="stylesheet" />
    <link href="../../Style/main.css" type="text/css" rel="stylesheet" />
    <style>
        body
        {
            background-image:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table cellspacing="0" cellpadding="0" width="90%" border="0" align="center">
            <tr>
                <td align="center">
                    <div style="width: 585px; position: relative; height: 400px; left: 0px; top: 0px;">
                        <asp:TextBox ID="tbName" 
                            Style="z-index: 155; left: 162px; position: absolute; top: 88px" Enabled="false"
                            runat="server" MaxLength="25" BorderWidth="0px" Width="300px" BorderStyle="Solid"
                            ForeColor="Black" ReadOnly="True"></asp:TextBox>
                        
                        <asp:TextBox ID="tbBonusScale" Style="z-index: 155; left: 162px; position: absolute;
                            top: 355px" runat="server" MaxLength="25" BorderWidth="1px" Width="100px"></asp:TextBox>
                        <asp:TextBox ID="tbContactPerson" runat="server" BorderWidth="1px" MaxLength="25"
                            Style="z-index: 155; left: 162px; position: absolute; top: 181px" Width="300px"
                            BorderStyle="Solid"></asp:TextBox>
                        <asp:TextBox ID="tbTelephone" runat="server" BorderWidth="1px" Style="z-index: 155;
                            left: 162px; position: absolute; top: 206px" Width="300px" BorderStyle="Solid"
                            ForeColor="Black"></asp:TextBox>
                         <asp:DropDownList ID="ddlCpsType" Enabled="false" Style="z-index: 155; left: 162px; position: absolute; 
                            top: 330px" runat="server" >
                            <asp:ListItem Value="1">推广员</asp:ListItem>
                            <asp:ListItem Value="2">代理商</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlCom" Style="z-index: 155; left: 162px; position: absolute; 
                            top: 380px" runat="server" >
                            <asp:ListItem Value="0">及时结算</asp:ListItem>
                            <asp:ListItem Value="1">月底结算</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="Label3" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 206px; right: 488px;">电话：</asp:Label>
                      
                        <asp:Label ID="Label6" Style="z-index: 155; left: 8px; position: absolute; top: 40px"
                            runat="server" Font-Bold="True" ForeColor="Green">代理商申请资料：</asp:Label>
                        <asp:Label ID="Label10" Style="z-index: 155; left: 62px; position: absolute; top: 88px"
                            runat="server">申请人：</asp:Label>
                        
                        <asp:Label ID="Label19" Style="z-index: 155; left: 61px; position: absolute; top: 355px"
                            runat="server">佣金：</asp:Label>
                        <asp:TextBox ID="tbMobile" runat="server" BorderWidth="1px" MaxLength="25" Style="z-index: 155;
                            left: 162px; position: absolute; top: 233px" Width="300px" BorderStyle="Solid"
                            ForeColor="Black"></asp:TextBox>
                        <asp:TextBox ID="tbEmail" runat="server" BorderWidth="1px" MaxLength="25" Style="z-index: 155;
                            left: 162px; position: absolute; top: 257px" Width="300px" BorderStyle="Solid"
                            ForeColor="Black"></asp:TextBox>
                        <asp:TextBox ID="tbQQ" runat="server" BorderWidth="1px" MaxLength="20" Style="z-index: 155;
                            left: 162px; position: absolute; top: 282px" Width="300px" 
                            BorderStyle="Solid"></asp:TextBox>
                       
                        <asp:TextBox ID="tbMD5Key" runat="server" BorderStyle="Solid" BorderWidth="1px"
                            MaxLength="128" Style="z-index: 155; left: 162px; position: absolute; top: 314px"
                            Width="307px"></asp:TextBox>
                            <asp:TextBox ID="tbUrlName" runat="server" BorderWidth="1px" MaxLength="5"
                            Style="z-index: 155; left: 162px; position: absolute; top: 136px" Width="300px"
                            BorderStyle="Solid"></asp:TextBox>
                            <asp:TextBox ID="tbUrl" runat="server" BorderWidth="1px" MaxLength="5"
                            Style="z-index: 155; left: 162px; position: absolute; top: 161px" Width="300px"
                            BorderStyle="Solid" ForeColor="Black"></asp:TextBox>
                        <asp:Label ID="Label11" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 307px">MD5私钥：</asp:Label>
                        <asp:Label ID="Label1" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 282px">QQ号码：</asp:Label>
                       
                        <asp:Label ID="Label5" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 233px">手机：</asp:Label>
                        <asp:Label ID="Label7" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 257px">电子邮件：</asp:Label>
                        <asp:Label ID="Label22" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 181px">联系人：</asp:Label>
                             <asp:Label ID="Label9" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 136px">网站名称：</asp:Label>
                             <asp:Label ID="Label12" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 161px">网址：</asp:Label>
                             <asp:Label ID="Label2" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 330px">CPS模式：</asp:Label>
                            <asp:Label ID="Label4" runat="server" Style="z-index: 155; left: 61px; position: absolute;
                            top: 380px">结算模式：</asp:Label>
                        <asp:CheckBox ID="cbON" Style="z-index: 153; left: 305px; position: absolute; top: 355px"
                            runat="server" Text="开通" Checked="True"></asp:CheckBox>
                        <asp:TextBox ID="tbID" runat="server" Visible="False"></asp:TextBox></div>
                </td>
            </tr>
            
            <tr>
                <td align="center">
                    <ShoveWebUI:ShoveConfirmButton ID="btnAccept" BackgroupImage="../../Images/Admin/buttbg.gif"
                        runat="server" Width="60px" Text="同意" AlertText="确认同意此代理商的申请吗？" Height="20px"
                        OnClick="btnAccept_Click" />
                    &nbsp;<ShoveWebUI:ShoveConfirmButton ID="btnNoAccept" runat="server" AlertText="确认要拒绝这个申请吗？"
                        Height="20px" BackgroupImage="../../Images/Admin/buttbg.gif" Text="拒绝" Width="60px"
                        OnClick="btnNoAccept_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <!--#includefile="~/Html/TrafficStatistics/1.htm"-->
</body>
</html>
