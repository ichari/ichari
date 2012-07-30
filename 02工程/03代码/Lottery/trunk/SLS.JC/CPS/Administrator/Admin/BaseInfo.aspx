<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BaseInfo.aspx.cs" Inherits="CPS_Administrator_Admin_BaseInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Style/css.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(about:blank);">
    <form id="form1" runat="server">
    <table width="770" border="0" align="right" cellpadding="0" cellspacing="0">
     <tr>
                        <td>
                            <table width="770" border="0" align="right" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                                <tr class="table">
                                    <td height="33" background="../../images/index-1_28.gif" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                        <table width="40%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="hui">
                                                    推广首页&nbsp;&gt;推广中心&nbsp;&gt;&nbsp;客户资料&nbsp;&gt;&nbsp;基本信息
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
                                        <strong class="blue12">基本信息</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" bgcolor="#FFFFFF" class="hui" style="padding: 10px;">
                                        <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ccccccc">
                                            <tr>
                                                <td height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    今日新增会员数：
                                                </td>
                                                <td height="28" align="left" bgcolor="#FFFFFF" class="red" style="padding-left: 10px;">
                                                    <span class="red" id="spanMemberCountByDay" runat="server">0</span><span class="hui_2">人</span>
                                                </td>
                                              
                                            </tr>
                                            <tr>
                                                <td width="23%" height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    累计会员人数：
                                                </td>
                                                <td width="77%" height="28" align="left" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <span class="red" id="spanMemberCount" runat="server">0</span><span class="hui_2">人</span>
                                                </td>
                                               
                                            </tr>
                                            
                                            <tr>
                                                <td height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    今日收入：
                                                </td>
                                                <td height="28" align="left" bgcolor="#FFFFFF" class="red" style="padding-left: 10px;">
                                                    <span class="red" id="spanIncomeByDay" runat="server">0</span><span class="hui_2">元</span>
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    累计收入：
                                                </td>
                                                <td height="28" align="left" bgcolor="#FFFFFF" class="red" style="padding-left: 10px;">
                                                    <span class="red" id="spanIncome" runat="server">0</span><span class="hui_2">元</span>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    已提佣金：
                                                </td>
                                                <td height="28" align="left" bgcolor="#FFFFFF" style="padding-left: 10px;">
                                                    <span class="red" id="spanUsedBonus" runat="server">0</span><span class="hui_2">元( 包含已经提款的金额、申请中的提款金额、提款手续费 )</span>
                                                </td>
                                             
                                            </tr>
                                            <tr>
                                                <td height="28" align="right" bgcolor="#F3F8FD" class="blue">
                                                    可提佣金：
                                                </td>
                                                <td height="28" align="left" bgcolor="#FFFFFF" class="hui_2" style="padding-left: 10px;">
                                                    <span class="red" id="spanAllowBonus" runat="server">0</span>元 (本月实时佣金<span id="lbThisMonthIncome" runat="server" class="red">0.0000</span> 元，未到结算期，不可提取)
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td height="40" colspan="2" align="center" bgcolor="#FFFFFF" class="blue">
                                                    <input  type="button" onclick="window.open('QueryBonusFlow.html')" value="查看CPS计算方式" />
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
