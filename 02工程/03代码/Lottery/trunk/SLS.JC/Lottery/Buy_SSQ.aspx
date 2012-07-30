<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Buy_SSQ.aspx.cs" Inherits="Lottery_Buy_SSQ" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<%@ Register Src="~/Home/Room/UserControls/UserMyIcaile.ascx" TagName="UserMyIcaile"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>双色球选号投注、合买－<%=_Site.Name %></title>
    <meta name="keywords" content="双色球选号投注、双色球合买、双色球玩法介绍" />
    <meta name="description" content="<%=_Site.Name %>提供双色球选号投注、合买、玩法介绍等服务。" />
    <script src="../JScript/Public.js" type="text/javascript"></script>
    <script src="JScript/buy_SSQ.js" type="text/javascript"></script>
    <script src="../JScript/Marquee.js" type="text/javascript"></script>
    <link href="style/Buy.css" rel="stylesheet" type="text/css" />
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link href="../Style/public.css" rel="stylesheet" type="text/css" />
    <link href="../Style/part_b.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../favicon.ico" />
    <style type="text/css">
        .style1
        {
            height: 22px;
        }
    </style>
</head>
<body class="gybg">
    <form id="form1" runat="server">
    <input id="tbPlayTypeID" name="tbPlayTypeID" type="hidden" />
    <asp:HiddenField ID="HidIsuseEndTime" runat="server" />
    <asp:HiddenField ID="HidIsuseID" runat="server" />
    <asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>

<div class="w980 png_bg2">
<!-- 内容开始 -->
<div class="w970">


    <div id="content">
        <div id="menu_right">
            <div id="cz_center">
                <!-- 期信息开始 -->
                <div style="border:#E3E3E4 1px solid; background-image: url(../Home/Room/Images/bg_hui.jpg);
                    background-repeat: repeat-x; background-position: top; background-color: #F9FDFF;
                    margin-bottom: 5px; height: 100%; overflow: hidden;">
                    <div style="float: left; width: 19%; text-align: center; vertical-align: middle;height: 100%; overflow: hidden;">
                        <img id="LotteryImg" src="../Home/Room/Images/logo_5.jpg" alt="双色球" width="105" height="108" />
                    </div>
                    <div style="float: left; width: 81%; margin-top: 0px;">
                        <div id="lastIsuseInfo" style="height: 100%; overflow: hidden;">
                            <img src='../Home/Room/Images/londing.gif' style="position: relative; left: 25%;" alt="" />
                        </div>
                        <div id="tdLotteryIntroduce" class="hui12" style="padding: 4px 0px 2px 0px;">
                            <h1 class='blue18' style="display: inline;">双色球：2元赢取￥1500万</h1>
                            &nbsp;&nbsp;每周二、四、日 21:30 开奖
                        </div>
                        <div class="black12" style="width: 100%; height: 24px; line-height: 24px;">
                            第 <span id='currIsuseName' class='red12' style='font-weight: bold;'></span> 期截止时间:&nbsp;
                            <span id="currIsuseEndTime" class="black12"></span>、离投注截止还有: 
                            <span id='toCurrIsuseEndTime' style='background-color: Black; font-weight: bold; color: #00FF00; padding: 2px;
                            font-size: 13px; font-family: 宋体; text-align: center; border-right: 1px solid red;'></span>
                            &nbsp;&nbsp;<span id="testNumber"></span>
                        </div>
                        <div class="red12" style="width: 100%; height: 24px; line-height: 24px;">
                            <a href="<%= ResolveUrl("~/TrendCharts/SSQ/SSQ_CGXMB.aspx") %>" target="_blank">走势图</a>&nbsp;&nbsp;
                            <a href="<%= ResolveUrl("~/WinQuery/5-0-0.aspx") %>" target="_blank">中奖查询</a>
                        </div>
                    </div>
                </div>
                <!-- 期信息结束 -->
                <!-- 中奖播报开始 -->
                <div id="divWinNotice" style="width: 100%;" runat="server" visible="false">
                    <div style="float: left; width: 14%; float: left; padding-left: 5px; height: 25px; background: url(../Home/Room/Images/ssl_zjbb_bg.gif)" class="red12_2">
                        中奖播报&nbsp;<img src="../Home/Room/Images/ssl_bb.gif" alt="" />
                    </div>
                    <div id="divSSQ" style="float: left; width: 85%;" runat="server" visible="false">
                        <iframe id="iframe2" name="iframeWinNotice" width="100%" height="25px" frameborder="0"
                            scrolling="no" src="../Home/Room/SSQ_WinNotice.aspx"></iframe>
                    </div>
                    <div id="div3D" style="float: left; width: 85%;" runat="server" visible="false">
                        <iframe id="iframe3" name="iframeWinNotice" width="100%" height="25px" frameborder="0"
                            scrolling="no" src="../Home/Room/3D_WinNotice.aspx"></iframe>
                    </div>
                </div>
                <!-- 中奖播报结束 -->
                <!-- 选项卡开始 -->
                <div id="TabMenu" style="margin-top: 15px; text-align: center;padding-bottom: 0px; width: 100%;">
                    <div style="float:left;width:1px;">1</div>
                    <div class="redMenu" onclick="newBuy(<%= LotteryID %>);">
                        选号投注
                    </div>
                    <div style="float: left; width: 1px;">1</div>
                    <div class="whiteMenu" onclick="schemeAll(<%=LotteryID %>);">
                        全部方案</div>
                    <div style="float: left; width: 1px;">1</div>
                    <div class="whiteMenu" onclick="followScheme(<%=LotteryID %>);">
                        定制跟单</div>
                    <div style="float: left; width: 1px;">1</div>
                    <div class="whiteMenu" onclick="PlayTypeIntroduce(<%=LotteryID %>);">
                        <strong>玩法介绍</strong></div>
                </div>
                <!-- 选项卡结束 -->
                <div id="divNewBuy">
                    <table width="100%" cellspacing="1" cellpadding="0" bgcolor="#E3E3E4" style="margin-top: 10px;">
                        <tbody>
                            <tr>
                                <td width='70' height='28' align='center' bgcolor='#F7FCFF' class='black12'>选择玩法</td><td bgcolor='#FFFFFF' class='black12' style='padding-left: 5px;'>
                                    <input type='radio' name='playType' id='playType501' value='501' checked='checked' onclick='clickPlayType(this.value)' />单式
                                    <input type='radio' name='playType' id='playType502' value='502' onclick='clickPlayType(this.value)' />复式
                                    <%--
                                    <input type='radio' name='playType' id='playType503' value='503' onclick='clickPlayType(this.value)' />胆拖
                                    <input type='radio' name='playType' id='playType504' value='504' onclick='clickPlayType(this.value)' /><span class='blue12'>智能机选</span><sup><img src='../Home/Room/Images/ico_new.gif'></sup>
                                    --%>
                                </td>
                            </tr>
                        </tbody>
                        <tr>
                            <td height="30" align="center" bgcolor="#F7FCFF" class="black12">
                                选号
                            </td>
                            <td valign="top" bgcolor="#FFFFFF">
                                <iframe id="iframe_playtypes" name="iframe_playtypes" width="100%" frameborder="0" scrolling="no" onload="document.getElementById('iframe_playtypes').height=iframe_playtypes.document.body.scrollHeight;"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" bgcolor="#ECF9FF" class="black12">
                                投注内容
                            </td>
                            <td valign="top" bgcolor="#FFFFFF">
                                <table width="95%" align="center" cellpadding="0" cellspacing="0" style="margin-top: 10px;">
                                    <tr>
                                        <td width="426" valign="top">
                                            <select size="5" name="list_LotteryNumber" multiple="multiple" id="list_LotteryNumber" style="width: 100%; height: 150px;"></select>
                                        </td>
                                        <td width="134" style="padding-left: 10px;">
                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td height="30">
                                                        <input id="btn_2_Rand" name="btn_2_Rand" type="button" value="机选1注" onclick="if(this.disabled){this.style.cursor='';return false;}return iframe_playtypes.btn_2_RandClick();" style="cursor: pointer; width: 80px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="30">
                                                        <input id="btn_2_Rand5" name="btn_2_Rand5" type="button" value="机选5注" onclick="if(this.disabled){this.style.cursor='';return false;}return iframe_playtypes.btn_2_RandManyClick(5);" style="cursor: pointer; width: 80px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="30">
                                                        <input id="btnPaste" name="btnPaste" type="button" value="粘贴上传" onclick="CreateUplaodDialog();" style="cursor: pointer; width: 80px;" />
                                                        <span id="spanJX" style="display: none">
                                                            <input id="tbJXNumbers" type="text" value="5" maxlength="3" style="width: 22px" onkeypress="return InputMask_Number();" />注
                                                            <input id="btn_2_Randn" style="color: Blue" name="btn_2_Randn" type="button" value="机选" style="cursor: pointer; width: 40px;" class="red2"
                                                                onclick="if(this.disabled){this.style.cursor='';return false;}if(StrToInt(tbJXNumbers.value)<1) {alert('请输入注数！'); tbJXNumbers.focus();return false;}else{return iframe_playtypes.btn_2_RandManyClick(StrToInt(tbJXNumbers.value));}" />
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="30">
                                                        <input type="button" name="btn_ClearSelect" id="btn_ClearSelect" value="删除选择" style="cursor: pointer;
                                                            width: 80px;" onclick="return btn_ClearSelectClick();" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="30">
                                                        <input type="button" name="btn_Clear" id="btn_Clear" value="清空全部" style="cursor: pointer;
                                                            width: 80px;" onclick="return btn_ClearClick();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="95%" align="center" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC"
                                    style="margin-top: 10px; margin-bottom: 10px;">
                                    <tr>
                                        <td height="30" bgcolor="#F9F9F9" class="hui12" style="padding-left: 8px;">
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        您选择了 <span class="red12" id="lab_Num">0</span> 注，倍数：
                                                    </td>
                                                    <td>
                                                        <input type="text" onkeypress="return InputMask_Number();" id="tb_Multiple" name="tb_Multiple"
                                                            onblur="CheckMultiple();" value="1" maxlength="3" style="width: 30px;" />
                                                    </td>
                                                    <td>
                                                        &nbsp;总金额 <span class="red12" id="lab_SumMoney">0.00</span> 元。<span class="red12">【注】</span>投注倍数最高为
                                                        999 倍。
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="center" bgcolor="#FEDEDE" class="black12">
                                合买代购
                            </td>
                            <td bgcolor="#FEECEC" class="red12" style="padding-left: 5px;">
                                <div id="DivBuy" style="float: left;">
                                    <input id="CoBuy" name="CoBuy" type="checkbox" onclick="oncbInitiateTypeClick(this);"
                                        value="2" />我要发起合买方案 <span style="margin-left: 50px;"></span>
                                </div>
                                <div id="DivChase" style="float: left;">
                                    <input id="Chase" name="Chase" type="checkbox" onclick="oncbInitiateTypeClick(this);"
                                        value="1" />我要追号</div>
                            </td>
                        </tr>
                        <tbody id="trShowJion" style="display: none; line-height: 2; height: 36px; background-color: #FFEEEE;
                            text-align: center; padding-left: 10px; padding-right: 10px;">
                            <tr>
                                <td>
                                    佣金比率
                                </td>
                                <td align="left">
                                    <input type="text" onkeypress="return InputMask_Number();" id="tb_SchemeBonusScale"
                                        name="tb_SchemeBonusScale" onblur="return SchemeBonusScale();" style="width: 56px;"
                                        maxlength="10" />% &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;佣金比例只能为0~10。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    我要分成
                                </td>
                                <td align="left">
                                    <input type="text" onkeypress="return InputMask_Number();" id="tb_Share" name="tb_Share"
                                        onblur="return CheckShare();" style="width: 56px;" maxlength="10" value="1" />份，每份
                                    <span id="lab_ShareMoney" style="color: Red">0.00</span>&nbsp;元。&nbsp;&nbsp; <font
                                        color="#ff0000">【注】</font>份数不能为空，且能整除总金额。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    我要认购
                                </td>
                                <td align="left">
                                    <input type="text" onkeypress="return InputMask_Number();" id="tb_BuyShare" name="tb_BuyShare"
                                        onblur="return CheckBuyShare();" style="width: 56px;" value="1" />份，金额 <span id="lab_BuyMoney"
                                            style="color: Red">0.00</span>&nbsp;元。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    我要保底
                                </td>
                                <td align="left">
                                    <input type="text" onkeypress="return InputMask_Number();" id="tb_AssureShare" name="tb_AssureShare"
                                        onblur="return CheckAssureShare();" style="width: 56px;" value="0">份，保底 <span id="lab_AssureMoney"
                                            style="color: Red">0.00</span>&nbsp;元。&nbsp; <font color="#ff0000">【注】</font>保底资金将被暂时冻结,在当期截止销售时、或根据此方案的销售、撤单情况,冻结资金将返还到您的电话投注卡账户中。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    方案标题
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_Title" name="tb_Title" style="width: 99%;" maxlength="50" /><font
                                        color="#ff0000">【注】</font>长度不能超过 <font color="#ff0000">50</font> 个字符。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    方案描述
                                </td>
                                <td align="left" style="padding: 10px;">
                                    <textarea id="tb_Description" name="tb_Description" style="width: 99%; height: 100px;"></textarea>
                                </td>
                            </tr>
                        </tbody>
                        <tbody id="trGoon" style="display: none; line-height: 2; height: 36px; background-color: #FFF3F3;
                            text-align: center; padding-left: 10px; padding-right: 10px;">
                            <tr>
                                <td>
                                    追号期间
                                </td>
                                <td align="left" style="padding: 10px;">
                                    <div>
                                        <table cellpadding="0" cellspacing="1" style="width: 100%; text-align: center; background-color: #E2EAED;">
                                            <tbody style="background-color: White;">
                                                <tr>
                                                    <td style="width: 10%;">
                                                        <input type="checkbox" name="cb_All" id="cb_All" onclick="Cb_CheckAll();" />选择
                                                    </td>
                                                    <td style="width: 40%;">
                                                        期号
                                                    </td>
                                                    <td style="width: 20%;">
                                                        投注倍数
                                                    </td>
                                                    <td style="width: 30%;">
                                                        购买金额
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div id="div_QH_Today" style="height: 200px; overflow: scroll; width: 100%; overflow-x: hidden;">
                                            <table id="RpToday" cellpadding="0" cellspacing="1" style="width: 100%; text-align: center;
                                                background-color: #E2EAED;">
                                                <tbody style="background-color: White;">
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    追号金额
                                </td>
                                <td align="left">
                                    任务总金额 <span id="LbSumMoney" style="color: red;" text="0.00"></span>&nbsp;元
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td>
                                    佣金比率
                                </td>
                                <td align="left">
                                    <input type="text" onkeypress="return InputMask_Number();" id="tb_SchemeBonusScalec"
                                        name="tb_SchemeBonusScalec" onblur="return SchemeBonusScalec();" style="width: 56px;"
                                        maxlength="10" />% &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;佣金比例只能为0~10。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    自动停止
                                </td>
                                <td align="left">
                                    当追号任务某期中奖金额达到
                                    <input type="text" id="tbAutoStopAtWinMoney" name="tbAutoStopAtWinMoney" maxlength="4"
                                        onkeypress="return InputMask_Number();" value="1" style="width: 60px;" />
                                    元时，系统<span style="color: #ff0000">中奖后自动停止</span>此追号任务。为<span style="color: #ff0000">&nbsp;0&nbsp;</span>时表示不启动自动终止功能。
                                </td>
                            </tr>
                        </tbody>
                        <tr>
                            <td height="30" align="center" bgcolor="#F7FCFF" class="black12">
                                方案保密
                            </td>
                            <td valign="middle" bgcolor="#FFFFFF" style="padding-left: 5px;">
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel0" value="0" checked="checked" />
                                <span>不保密</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel1" value="1" />
                                <span>保密到截止</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel2" value="2" />
                                <span>保密到开奖</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel3" value="3" />
                                <span>永久保密</span>
                            </td>
                        </tr>
                     
                        <tr>
                            <td height="30" colspan="2" bgcolor="#F7F7F7" align="center" style="padding-bottom: 20px;
                                padding-top: 20px">
                                <ShoveWebUI:ShoveConfirmButton ID="btn_OK" Style="cursor: pointer;" runat="server"
                                    Width="197px" Height="33px" CausesValidation="False" BackgroupImage="../Home/Room/Images/zfb_bt_touzhu.jpg"
                                    BorderStyle="None" OnClientClick="return CreateLogin('btn_OKClick();');" OnClick="btn_OK_Click" />
                                <asp:CheckBox ID="chkAgrrement" runat="server" Checked="true" />
                                我已经阅读并同意 <span class="blue12">
                                    <asp:HyperLink runat="server" ID="hlAgreement" Target="_blank">《用户电话短信投注协议》</asp:HyperLink></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divCoBuy" style="display: none;">
                    <iframe id="iframeCoBuy" name="iframeCoBuy" width="100%" frameborder="0" scrolling="no"
                        onload="document.getElementById('iframeCoBuy').height=iframeCoBuy.document.body.scrollHeight;">
                    </iframe>
                </div>
                <div id="divFollowScheme" style="display: none;">
                    <iframe id="iframeFollowScheme" name="iframeFollowScheme" width="100%" frameborder="0"
                        scrolling="no" onload="document.getElementById('iframeFollowScheme').height=iframeFollowScheme.document.body.scrollHeight;">
                    </iframe>
                </div>
                <div id="divSchemeAll" style="display: none;">
                    <div id="divLoding" style="line-height: 35px; height: 100%; overflow: hidden;">
                        <img src='../Home/Room/images/londing.gif' style="position: relative; top: 10%; left: 40%;" alt="" />
                    </div>
                    <iframe id="iframeSchemeAll" name="iframeSchemeAll" width="100%" frameborder="0"
                        scrolling="no" onload="$Id('iframeSchemeAll').style.display ='';document.getElementById('divLoding').style.display='none';document.getElementById('iframeSchemeAll').height=iframeSchemeAll.document.body.scrollHeight;">
                    </iframe>
                </div>
                <div id="divPlayTypeIntroduce" style="display: none;">
                    <iframe id="iframePlayTypeIntroduce" name="iframePlayTypeIntroduce" width="100%"
                        frameborder="0" scrolling="no" onload="document.getElementById('iframePlayTypeIntroduce').height=iframePlayTypeIntroduce.document.body.scrollHeight;">
                    </iframe>
                </div>
            </div>
            <div id="cz_right">
                <!-- 幸运号码 开始-->
                <div id="topxy" class="hidden">
                    <div style="width: 218px; background-image: url(../Home/Room/Images/title_bg_blue.jpg);
                        border: solid 1px #BCD2E9; overflow: hidden;">
                        <div style="width: 100%; line-height: 28px; margin-left: 15px;" class="blue14"">
                            <strong>幸运号码</strong>
                        </div>
                    </div>
                    <div style="width: 218px; border: solid 1px #BCD2E9; border-top: none; text-align: center;
                        height: 100%; overflow: hidden; margin-bottom: 10px;">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" background="../Home/Room/Images/ssq_qh_bg.jpg">
                            <tr>
                                <td width="6" height="29">
                                    &nbsp;
                                </td>
                                <td width="40" align="center" background="../Home/Room/Images/ssq_qh_1.jpg" class="blue12">
                                    <a href="javascript:;" id="hrefXZ" onclick="return ClickXYJX(this,1)">星座</a>
                                </td>
                                <td width="4">
                                    &nbsp;
                                </td>
                                <td width="40" align="center" class="blue12">
                                    <a href="javascript:;" id="hrefSX" onclick="return ClickXYJX(this,2)">生肖</a>
                                </td>
                                <td width="4">
                                    &nbsp;
                                </td>
                                <td width="70" align="center" class="blue12">
                                    <a href="javascript:;" id="hrefCSRQ" onclick="return ClickXYJX(this,3)">出生日期</a>
                                </td>
                                <td width="4">
                                    &nbsp;
                                </td>
                                <td width="40" align="center" class="blue12">
                                    <a href="javascript:;" id="hrefXM" onclick="return ClickXYJX(this,4)">姓名</a>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="background-repeat: repeat-x;
                            background-position: center bottom; background-image: url(../Home/Room/Images/ssq_bg_di.jpg)">
                            <tr>
                                <td style="padding: 10px;">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="48%" height="30" align="left">
                                                <span id="spanXZ">
                                                    <asp:DropDownList ID="ddlXiZuo" runat="server">
                                                        <asp:ListItem Value="1">白羊座</asp:ListItem>
                                                        <asp:ListItem Value="2">金牛座</asp:ListItem>
                                                        <asp:ListItem Value="3">双子座</asp:ListItem>
                                                        <asp:ListItem Value="4">巨蟹座</asp:ListItem>
                                                        <asp:ListItem Value="5">狮子座</asp:ListItem>
                                                        <asp:ListItem Value="6">处女座</asp:ListItem>
                                                        <asp:ListItem Value="7">天秤座</asp:ListItem>
                                                        <asp:ListItem Value="8">天蝎座</asp:ListItem>
                                                        <asp:ListItem Value="9">射手座</asp:ListItem>
                                                        <asp:ListItem Value="10">摩羯座</asp:ListItem>
                                                        <asp:ListItem Value="11">水瓶座</asp:ListItem>
                                                        <asp:ListItem Value="12">双鱼座</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span><span id="spanSX" style="display: none">
                                                    <asp:DropDownList ID="ddlSX" runat="server">
                                                        <asp:ListItem Value="1">鼠</asp:ListItem>
                                                        <asp:ListItem Value="2">牛</asp:ListItem>
                                                        <asp:ListItem Value="3">虎</asp:ListItem>
                                                        <asp:ListItem Value="4">兔</asp:ListItem>
                                                        <asp:ListItem Value="5">龙</asp:ListItem>
                                                        <asp:ListItem Value="6">蛇</asp:ListItem>
                                                        <asp:ListItem Value="7">马</asp:ListItem>
                                                        <asp:ListItem Value="8">羊</asp:ListItem>
                                                        <asp:ListItem Value="9">猴</asp:ListItem>
                                                        <asp:ListItem Value="10">鸡</asp:ListItem>
                                                        <asp:ListItem Value="11">狗</asp:ListItem>
                                                        <asp:ListItem Value="12">猪</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span><span id="spanCSRQ" style="display: none">
                                                    <asp:TextBox ID="tbDate" runat="server" Width="80" CssClass="hui12" ToolTip="格式：1980-01-01"
                                                        Text="1980-01-01" onfocus="this.className='';this.value='';" onblur="if(this.value==''){this.className='hui12'; this.value='1980-01-01';}"></asp:TextBox>
                                                </span><span id="spanXM" style="display: none">
                                                    <asp:TextBox ID="tbName" runat="server" Width="80" CssClass="hui12" ToolTip="支持中英文"
                                                        Text="支持中英文" onfocus="if(this.value=='支持中英文') {this.className='';this.value='';}"
                                                        onblur="if(this.value=='') {this.className='hui12';this.value='支持中英文';}"></asp:TextBox>
                                                </span>
                                            </td>
                                            <td width="52%">
                                                <img src="../Home/Room/Images/ssq_bt_1.jpg" width="100" height="21" alt="" border="0"
                                                    style="cursor: pointer" onclick="return GetLuckNumber(<%=LotteryID %>)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" runat="server" id="tdLuckNumber">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center" style="padding-top: 15px; padding-bottom: 5px">
                                                <img src="../Home/Room/Images/ssq_bt_2.jpg" width="140" height="30" alt="" border="0"
                                                    style="cursor: pointer" onclick="return BetLuckNumber(<%=LotteryID %>);" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!-- 幸运号码 结束-->
                <!-- 福彩专家栏 开始-->
                <!--div id="FC">
                    <div style="width: 218px;  border: solid 1px #e3e3e4;color:#FFF; line-height:32px;" class="bg1x bgp3">
                        <div style="float: left; width: 132px; line-height: 28px; padding-left: 15px;" class="blue12_1">
                            <strong>福彩专家栏</strong>
                        </div>
                        <div style="float: left; width: 65px; margin-top: 5px; text-align: right;" class="hui12">
                            <span style="cursor: pointer" onclick="BindFCExpertList(0);">
                                <img src='../Home/Room/Images/page_first.gif' width='9' alt="" height='8' /></span>
                            <span style="cursor: pointer" onclick="BindFCExpertList(1);">
                                <img src='../Home/Room/Images/page_previous.gif' width='9' height='8' alt="" /></span><span
                                    style="padding-left: 10px; cursor: pointer" onclick="BindFCExpertList(2);"><img src='../Home/Room/Images/page_next.gif'
                                        width='9' alt="" height='8' /></span> <span style="cursor: pointer" onclick="BindFCExpertList(3);">
                                            <img src='../Home/Room/Images/page_last.gif' width='9' alt="" height='8' /></span>
                        </div>
                    </div>
                    <div id="ExpertList" style="width: 210px; border: solid 1px #e3e3e4; border-top: none;
                        text-align: center; height: 100%; overflow: hidden; padding: 4px;">
                        <br />
                        <img src='../Home/Room/Images/londing.gif' alt="" />
                        <br />
                        <br />
                        <br />
                    </div>
                </div>
                <!-- 福彩专家栏 结束-->
               
                <table width="220" id="tdNumber" runat="server" class="hidden"  border="0" cellspacing="1"
                    cellpadding="0" style="margin-bottom: 10px;" bgcolor="#BCD2E9">
                    <tr>
                        <td width="100%" height="28" style="padding-left: 15px; background-color: #E7F1FA;">
                            <table width="120" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="77%" class="blue14 ">
                                        每期奖不停
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#FFFFFF" width="100%" height="25px" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td id="divNumber" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!--每期奖不停结束-->
                <!-- 开奖列表开始-->
                <div style="width: 218px;  border: solid 1px #e3e3e4;color:#FFF; line-height:32px; margin-top:0;" class="bsc3 bg1x bgp3">
                    <div style="float: left; width: 132px; line-height: 28px; padding-left: 15px;" class="blue12_1">
                        <strong>开奖号码</strong>
                    </div>
                    <div style="float: left; width: 65px; margin-top: 5px; text-align: right;" class="hui12">
                        <span style="cursor: pointer" onclick="BindWinNumber(0);">
                            <img src='../Home/Room/Images/page_first.gif' width='9' alt="" height='8' /></span>
                        <span style="cursor: pointer" onclick="BindWinNumber(1);">
                            <img src='../Home/Room/Images/page_previous.gif' width='9' height='8' alt="" /></span><span
                                style="padding-left: 10px; cursor: pointer" onclick="BindWinNumber(2);"><img src='../Home/Room/Images/page_next.gif'
                                    width='9' alt="" height='8' /></span> <span style="cursor: pointer" onclick="BindWinNumber(3);">
                                        <img src='../Home/Room/Images/page_last.gif' width='9' alt="" height='8' /></span>
                    </div>
                </div>
                <div id="tdWinLotteryNumber" style="width: 210px; border: solid 1px #e3e3e4; border-top: none;
                    text-align: center; height: 100%; overflow: hidden; margin-bottom: 10px; padding: 4px;">
                    <br />
                    <img src='../Home/Room/Images/londing.gif' alt="" />
                    <br />
                    <br />
                    <br />
                </div>
                <!-- 开奖列表结束-->
              
                
                
                <!-- 中奖排行榜 开始-->
                <div style="width: 218px;  border: solid 1px #e3e3e4;color:#FFF; line-height:32px;" class="bsc3 bg1x bgp3">
                    <div style="width: 100%; line-height: 28px; float: left; padding-left: 15px;" class="blue12_1">
                        <strong>中奖排行榜</strong>
                    </div>
                   
                </div>
                <div style="width: 210px; border: solid 1px #e3e3e4; border-top: none; height: 100%;
                    overflow: hidden; margin-bottom: 10px; padding: 4px;">
                    <table cellspacing="1" cellpadding="0" style="text-align: center; margin: 1%;" width="98%">
                        <tbody id="tbWin1" runat="server">
                        </tbody>
                      
                    </table>
                </div>
                <!-- 中奖排行榜 结束-->
            </div>
        </div>
    </div>

</div>

<!-- 内容结束 -->
</div>
<div class=" w980 png_bg1"></div>

    <asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
    <input id="tbPlayTypeName" name="tbPlayTypeName" type="hidden" value="单式" />
    <input id="tb_LotteryNumber" name="tb_LotteryNumber" type="hidden" />
    <input id="tb_hide_SumMoney" name="tb_hide_SumMoney" type="hidden" />
    <input id="tb_hide_AssureMoney" name="tb_hide_AssureMoney" type="hidden" />
    <input id="tb_hide_SumNum" name="tb_hide_SumNum" type="hidden" />
    <input id="HidMaxTimes" name="HidMaxTimes" type="hidden" value="1000" />
    <input id="HidLotteryID" name="HidLotteryID" type="hidden" value="<%=LotteryID %>" />
    <input id="HidPrice" name="HidPrice" type="hidden" value="2" />
    <input id="tb_hide_ChaseBuyedMoney" name="tb_hide_ChaseBuyedMoney" type="hidden" />
    <asp:HiddenField ID="HidSchemeUpload" runat="server" />
    <asp:HiddenField ID="HidType" runat="server" Value="1" />
    <asp:HiddenField ID="HidLuckNumber" runat="server" />
    <input id="HidSelectedLotteryNumber" name="HidSelectedLotteryNumber" type="hidden" />
    
    </form>
    <!--#includefile="../Html/TrafficStatistics/1.htm"-->
</body>
</html>

<script type="text/javascript">
    var sampleDiv = new scrollingAD("divwen");
    sampleDiv.width = 218;
    sampleDiv.height = 22;
    sampleDiv.delay = 20;
    sampleDiv.pauseTime = 5000;
    sampleDiv.move();
    // ]]>
</script>

<script type="text/javascript">
    
    Page_load(<%= LotteryID %>);
    PageEvent(<%=LotteryID %>);
    <%=DZ %>
    
</script>

<%= script %>