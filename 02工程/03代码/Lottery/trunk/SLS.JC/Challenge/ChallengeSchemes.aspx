<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChallengeSchemes.aspx.cs"
    Inherits="Challenge_ChallengeSchemes" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>方案详情</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="Style/ring.css" />
</head>
<body style="font-size: 12px">
    <div class="popuptab">
        <form id="form1" runat="server">
        <div style="font-family:微软雅黑">
            <table class="table_c1" name="table_s" cellspacing="1" cellpadding="0" width="100%"
                align="center" border="0">
                <tbody>
                    <tr>
                        <td colspan="11" class="pop_hh">
                            <a href="#" target="_blank" class="dl"><strong>
                                <%=UserName%></strong></a><strong> 的竞猜方案 (发起时间：<%=BetTime%>)</strong>
                        </td>
                    </tr>
                    <tr class="tre">
                        <td width="13%">
                            编号
                        </td>
                        <td width="9%">
                            赛事
                        </td>
                        <td width="15%">
                            比赛时间
                        </td>
                        <td>
                        主队
                        </td>
                        <td>
                        让球
                        </td>
                        <td>
                        客队
                        </td>
                        <td width="60px">
                            胜
                        </td>
                        <td width="60px">
                            平
                        </td>
                        <td width="60px">
                            负
                        </td>
                    </tr>
                    <%=sbHtml %>
                </tbody>
            </table>
            <div style="padding: 6px 10px; height: 32px">
                <span style="float: right">
                    <img src="images/btn_close.gif" onclick="javascirpt:window.close()" onmouseover="this.style.cursor='pointer'"
                        width="87" height="30" style="margin-left: 10px" />
                </span>
                <p id="countsNum" style="padding-top: 8px">
                    过关方式：<span style="color: Red; font-weight: bold;"><%=Way%></span>，场数：<span style="color: Red;
                        font-weight: bold;"><%=BetCount %></span> 场，奖金预测：<span style="color: Red; font-weight: bold;"><%=WinMoney%></span>元</p>
                <input name="spfnum" id="spfnum" type="hidden" value="9180,1,'2011-6-17 22:50:00';9190,3,'2011-6-19 0:50:00'" />
            </div>
        </div>
        </form>
    </div>
</body>
</html>
