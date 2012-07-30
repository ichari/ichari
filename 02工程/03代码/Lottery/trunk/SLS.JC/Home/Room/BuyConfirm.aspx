<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuyConfirm.aspx.cs" Inherits="Home_Room__BuyConfirm"%>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<%@ Register Src="~/Home/Room/UserControls/WebHead.ascx" TagName="WebHead" TagPrefix="uc1" %>
<%@ Register Src="~/Home/Room/UserControls/WebFoot.ascx" TagName="WebFoot" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <link href="../../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
body,ul,ol,li,p,h1,h2,h3,h4,h5,h6,form,table,tr,th,td,img,em,dd,dl,dt {margin:0; padding:0; border:0;}
body {font-size:12px; color:#333;}
table {font-family:Tahoma,Helvetica,Arial,"宋体",sans-serif;}
th {font-weight:normal;}
ul,ol {list-style-type:none;}
input,slect,img {vertical-align:middle;}
#wrap {width:1002px; margin:0 auto; margin-top:10px;}

#gc_bg,.nav_button,.choice,.gg_form_tr_01,.gg_form_tr_02,.form_tr,.listh1 {background:url(/Images/linebg2.png) repeat-x;}.butt1 a,.butt2 a,.listh1 span {background:url(/Images/trade.png) no-repeat;}

/* 选择玩法 */
.touzhu {border:solid #A4CFF1 1px;}
.choice {height:28px; padding-top:6px; border:solid #DADADA; border-width:0 1px; padding-left:10px; background-position:0 -6px;}
.reply_type_01 {height:28px; padding-top:5px; padding-left:10px; position:relative; }
.ss_slect {position:absolute; margin:-5px 0 0 10px;}

/* 确认代购 */
.listh1 {height:33px; background-position:0 -183px;}
.listh1 span {width:94px; margin-left:20px; height:33px; display:block; background-position:0 -375px; text-indent:-9999px;}
.listtable_t { height:30px; background:#F3F3F3; border-bottom:#E4E4E4 solid 1px; border-top:#fff solid 1px; text-align:center; padding:5px 10px; width:130px;}
.listtable_c { height:35px; background:#fff; border-bottom:#E4E4E4 solid 1px; border-top:#fff solid 1px; padding:5px 10px;}
.none_line {border-bottom:none;}
.dg_trbg1 {line-height:30px; text-align:center; background:#F8F7F1;}
.dg_trbg2 {line-height:30px; text-align:center; background:#fff;}
.gg_form_trt_01 td {height:24px; color:#4E672D; text-align:center;}
.gg_form_trt_02 td {height:24px; background:#C1E591; color:#4E672D; text-align:center}
.red{font-size: 14px;color: #cc0000;font-family: "tahoma";line-height: 25px;font-weight: bold;}
.BlueLightBg {background-color: #79B2F1;}
.WhiteWords {font-size: 12px;line-height: 16px;color: white;text-decoration: none;}
.WhiteBg {background-color: white;}
.BlueLightBg2 {background-color: #E9F3FF;}
.BlueWord {font-size: 12px;line-height: 18px;color: #182A6C;text-decoration: none;}
.BgBlue {font-size: 12px;background-color: #5C8FD8;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:WebHead ID="WebHead1" runat="server" />
        <div id="wrap">
            <div class="touzhu">
                <h3 class="listh1">
                    <span>确认投注内容</span></h3>
                    <table class="listtable" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td width="143" class="listtable_t">
                                方案截止时间
                            </td>
                            <td class="listtable_c">
                                <asp:Label ID="labEndTime" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="listtable_t" style="width: 100px;">
                                投注内容
                            </td>
                            <td class="listtable_c">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tbody><tr><td style="padding-right: 5px;" valign="top">
                                                <asp:Label ID="labLotteryNumber" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td> <a href="Scheme.aspx">查看方案详细信息</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="listtable_t" style="width: 100px;">
                                方案信息
                            </td>
                            <td class="listtable_c" style="line-height: 22px;">
                                &nbsp;--方案注数<asp:Label ID="labNum" runat="server" class="red"></asp:Label>注，倍数<asp:Label ID="labMultiple" runat="server" class="red"></asp:Label>倍，总金额￥<asp:Label ID="labSchemeMoney" runat="server" class="red"></asp:Label>元。
                            </td>
                        </tr>
                        <tr>
                            <td class="listtable_t none_line" style="width: 100px;">
                                &nbsp;
                            </td>
                            <td class="listtable_c none_line" style="padding-bottom: 20px;">
                                <br />
                                <ShoveWebUI:ShoveConfirmButton ID="btnOK" BackgroupImage="/Images/btn_sms.gif" runat="server" Style="cursor: pointer;" Height="37" Width="143" OnClick="btnOK_Click" AlertText="确认要提交方案吗?" />
                                <br />
                                <br />
                                <input id="agreement" checked="checked" type="checkbox" />
                                <label for="agreement">
                                    我已阅读了<a href="/Home/Room/BuyProtocol.aspx?LotteryID=72" target="_blank">《用户合买代购协议》</a>并同意其中条款。</label>
                                <br />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <uc3:WebFoot ID="WebFoot1" runat="server" />
        <input type='hidden' name='hidSchemeMoney' id="hidSchemeMoney" value='1' runat="server"/>
        <input type='hidden' name='hidMultiple' id="hidMultiple" value='1' runat="server"/>
        <input type='hidden' name='hidlotid' id="hidlotid" value='1' runat="server"/>
        <input type='hidden' name='hidplayid' id="hidplayid" value='1' runat="server"/>
        <input type='hidden' name='hidisuseid' id="hidisuseid" value='1' runat="server"/>
        <input type='hidden' name='hidSumNum' id="hidSumNum" value='1' runat="server"/>
        <input type='hidden' name='HidIsuseEndTime' id="HidIsuseEndTime" value='1' runat="server"/>
        <input type='hidden' name='hidMatchID' id="hidMatchID" value='1' runat="server"/>
        <input type='hidden' name='hidBuyID' id="hidBuyID" value='0' runat="server"/>
        <input type='hidden' name='hidLotteryNumber' id="hidLotteryNumber" value='' runat="server"/>
    </div>
    </form>
</body>
</html>
<script language="javascript" type="text/javascript">
    function btn_OKClick() {
        __doPostBack('btnOK_Click', '');
    }    
</script>