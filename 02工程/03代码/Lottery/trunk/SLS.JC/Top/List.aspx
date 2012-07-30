<%@ Page Language="C#" AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="Top_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>湖北省体育彩票管理中心-跟单管家</title>
    <link href="../style/main.css" rel="stylesheet" type="text/css" />
    <link href="../style/paihang.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../JScript/boot.js"></script>
    <script language="javascript" type="text/javascript" src="../JScript/fra.js"></script>
    <link href="/Style/pub.css" rel="stylesheet" type="text/css" />
    <link href="/Style/thickbox.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
	function ShowMedal(fUserID,fLotID)
	{
	    var StrUrl = "../Home/Web/Score.aspx?id=" + fUserID + "&LotteryID=" + fLotID;
	    window.open(StrUrl,'_blank');
	}
    </script>

    <style type="text/css">
        .ssq_top3
        {
            background: none repeat scroll 0 0 #FEF2DC;
            border: 1px solid #F6CE86;
            color: #FF6600;
            display: block;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 10px;
            font-weight: bold;
            height: 14px;
            line-height: 14px;
            margin: 0 auto;
            text-align: center;
            width: 18px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="m_header">
    <div class="bq_02_dowm bq_02_top">
        <asp:Label ID= "lbtitle" runat="server"> </asp:Label></div>
    <div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table_list01">
            <!--list-->
            <asp:Label ID="lbMatch" runat="server"></asp:Label>
        </table>
        <!--分页-->
        <div class="clear">
        </div>
    </div>
    <!--未登录提示层-->
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
</html>
