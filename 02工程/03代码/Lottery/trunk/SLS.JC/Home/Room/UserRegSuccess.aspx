<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserRegSuccess.aspx.cs" Inherits="Home_Room_UserRegSuccess" %>
<%@ Register Src="UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户注册成功-<%=_Site.Name %>-手机买彩票，就上<%=_Site.Name %>！</title>
    <meta name="description" content="<%=_Site.Name %>彩票网是一家服务于中国彩民的互联网彩票合买代购交易平台，涉及中国彩票彩种最全的网站。"/>
    <meta name="keywords" content="用户注册成功" />
     
    <link href="Style/css.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            font-size: 12px;
            color: #226699;
            font-family: "tahoma";
            line-height: 20px;
            height: 28px;
        }
        A
        {
            text-decoration: none;
        }
        A:hover
        {
            color: #CC0000;
            text-decoration: underline;
        }
    
p.p0{
margin:0pt;
margin-bottom:0.0001pt;
margin-bottom:0pt;
margin-top:0pt;
text-align:justify;
font-size:10.5000pt; font-family:'Times New Roman'; }
    </style>
<link rel="shortcut icon" href="../../favicon.ico" /></head>
<body class="gybg">
    <form id="form1" runat="server">
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

    <div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">
    <div id="content">
        <div id="menu_left">
            <uc2:UserMyIcaile ID="UserMyIcaile1" runat="server" />
        </div>
        <div id="menu_right">
            <table width="832" border="0" cellspacing="1" cellpadding="0" bgcolor="#D8D8D8">
                <tr>
                    <td align="left" bgcolor="#FFFFDD" class="black12" style="padding: 5px 10px 5px 10px;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="9%">
                                    <img src="images/icon_cg222.jpg" width="73" height="52" />
                                </td>
                                <td width="91%" class="red14_2">
                                    <%=_User.Name%>，祝贺您注册成功！
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="832" border="0" cellspacing="0" cellpadding="0" style="margin-top: 15px;">
                <tr>
                    <td height="36" colspan="2" class="blue12_line">
                        <div id="hr">
                        </div>
                    </td>
                </tr>
                 <tr>
                    <td width="61%" height="36" class="black12" align="left" style="padding-left: 20px;">
                         <a href="SafeSet.aspx?FromUrl=UserRegSuccess.aspx">
                            <img src="images/set_QA.jpg" width="127" height="36" border="0" /></a>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        <!--a href="UserMobileBind.aspx">
                            <img src="images/sjyz_bt.jpg" width="127" height="36" border="0" /></a>
                            &nbsp;&nbsp;&nbsp;&nbsp;-->
                            <a href="UserEmailBind.aspx">
                            <img src="images/jh_bt.jpg" width="127" height="36" border="0" /></a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="padding-left:10px; color:Red; margin-top:10px; padding-bottom:10px;">
                        1. 强烈建议您激活邮箱，邮箱是找回密码的唯一途径<br>
                        <!---2. 强烈建议您验证手机，本网站会在购彩全程短信帮助提示--->
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height="36" colspan="2" class="blue12_line">
                        <div id="hr">
                        </div>
                    </td>
                </tr>
            </table>
          
        </div>
    </div>


    </div>
<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>
    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    </form>
    <!--#includefile="../../Html/TrafficStatistics/1.htm"-->
</body>
</html>
