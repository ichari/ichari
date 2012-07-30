<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Client_SFC.aspx.cs" Inherits="JCLC_Client_SFC" EnableViewState="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>让球胜平负-竞彩足球</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/tips_globle.css" rel="stylesheet" type="text/css" />
    <link href="Style/public.css" rel="stylesheet" type="text/css" />
    <link href="Style/trade.css" rel="stylesheet" type="text/css" />
    <link href="/Style/pub.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        table.showodds .odds, table.showrate .rate{display: table-cell;_display: block;}
        table.showodds .rate, table.showrate .odds{display: none;}
    </style>
</head>
<body>
    <form id="buy_form" runat="server" ajax="Buy_Handler.ashx">
  <div style="width:814px;">
        <div style="width:814px;">
            <!-- 开始选择 !-->
            <div class="touzhu" style="border-top-color: #FFF;">
                <div class="reply_type_01" style="display:none">
                    <p class="time_scroll">
                        <span class="time_scroll2" id="TimeDisplay"></span>
                    </p>
                    <img src="Images/btn_sai.gif" style="cursor: pointer;" onclick="document.getElementById('leagueBox').style.display='block'"
                        align="absmiddle" alt="赛事选择">
                    已隐藏<span class="red" id="hideCount">0</span>场比赛 <a href="javascript:void 0" id="showAllTeam">
                        <font color="#ff0000">恢复</font></a>
                </div>
                <div class="searchbg" style="display:none">
                </div>
                <div class="ss_slect" style="display:none">
                    <table style="border: 1px solid #1989D7; display: none;" id="leagueBox" width="115"
                        bgcolor="#ffffff" border="0" cellpadding="3" cellspacing="1">
                        <tbody>
                            <tr>
                                <td bgcolor="#D1E7FC" height="22">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    &nbsp;赛事选择
                                                </td>
                                                <td width="1" align="right">
                                                    <img src="Images/btnClose.gif" alt="关闭" style="cursor: pointer; padding-right: 3px;"
                                                        onclick="document.getElementById('leagueBox').style.display='none'" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" bgcolor="#f1f8fe" height="25">
                                    <img src="Images/select_all.gif" alt="全选" style="cursor: pointer;" id="selectAllBtn" /><img
                                        src="Images/select_alt.gif" alt="反选" style="cursor: pointer;" id="selectOppBtn" /><img
                                            src="Images/close2.gif" alt="关闭" style="cursor: pointer;" onclick="document.getElementById('leagueBox').style.display='none'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- 日期选择开始 -->
                <div style="position: absolute; left: 530px; top: 230px; display:none;">
                </div>
                <!-- 开始对阵列表 !-->
			<table class="ttd showodds" id="table_vs" width="100%" border="0" cellpadding="0" cellspacing="1">
				<tbody>
					<tr class="gg_form_tr_01_lq">
						<td width="70" rowspan="2">赛事编号</td>
						<td width="160" rowspan="2">赛事类型 </td>
						<td width="75" rowspan="2">截止时间</td>
						<td width="90" rowspan="2">客队</td>
						<td width="90" rowspan="2">主队</td>
						<td colspan="8" height="20">请选择投注</td>
					</tr>
					<tr class="gg_form_tr_02_lq">
						<td width="40" height="20" class="s">客/主</td>
						<td width="60" class="s">1-5</td>
						<td width="60" class="s">6-10</td>
						<td width="60" class="s">11-15</td>
						<td width="60" class="s">16-20</td>
						<td width="60" class="s">21-25</td>
						<td width="70" class="s">26+</td>
						<td width="35">全包</td>
					</tr>
                        <%= MatchList %>
                    </tbody>
                </table>
                <!-- start 合买区  !-->
                <table class="bd_touzhu blank" width="98%" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td valign="top" width="58%">
                                <table class="table_jczqtz_gg_lq" width="98%" border="0" cellpadding="4" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="jczqtouzu_title_gg_lq">
                                                <span class="bold">您选择的场次及投注选项</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="table_bdtz_gg_lq" width="100%" border="0" cellpadding="4" cellspacing="0">
                                                    <tbody>
                                                        <tr class="t02">
                                                            <td width="100" align="center">
                                                                赛事编号
                                                            </td>
                                                            <td align="center">
                                                                客队 VS 主队
                                                            </td>
                                                            <td align="center" class="bnone">
                                                                您的投注选项
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                    <tbody id="row_tpl" style="display:none;"><tr class="t03"><td><input checked="checked" value="2929" name="m2929" type="checkbox"><span>周四006</span></td><td align="center">巴兰基利亚青年 VS 蒙特维迪奥竞技</td><td align="center" class="bnone">
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="2" />主胜1-5</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="4" />主胜6-10</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="6" />主胜11-15</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="8" />主胜16-20</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="10" />主胜21-25</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="12" />主胜26+</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="1" />客胜1-5</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="3" />客胜6-10</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="5" />客胜11-15</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="7" />客胜16-20</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="9" />客胜21-25</label>
                                                    <label style="display:none"><input type="checkbox"checked="checked"value="11" />客胜26+</label>
                                                    </td></tr></tbody>
                                                    <tbody id="selectTeams">
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" height="50">
                                                <span style="line-height: 22px; cursor: pointer" id="clearAllSelect">
                                                    <img alt="清空投注" src="Images/b_img_014.gif" /></span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td valign="top">
                                <table class="table_jczqtz_gg_lq" style="" width="100%" border="0" cellpadding="4"
                                    cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td" class="jczqtouzu_title_gg_lq">
                                                请选择过关方式
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="table_bdtz_gg_lq" width="100%" border="0" cellpadding="4" cellspacing="0"
                                                    style="margin: 0">
                                                    <tbody>
                                                        <tr class="t04" style="display:none">
                                                            <td>
                                                                1、选择过关类型
                                                            </td>
                                                        </tr>
                                                        <tr class="t05">
                                                            <td>
                                                                <input name="ssq_dg" checked="checked" type="radio" />普通过关
                                                                <div style="display: none">
                                                                    <input name="ssq_dg" disabled="disabled" type="radio" />组合过关
                                                                    <input name="ssq_dg" disabled="disabled" type="radio" />自由过关
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="t04" style="display:none">
                                                            <td>
                                                                2、选择过关方式
                                                            </td>
                                                        </tr>
                                                        <tr class="t05">
                                                            <td id="ggList" align="left" bgcolor="#ffffff">
                                                                <div>
                                                                    <label style="display: none;" for="r2c1">
                                                                        <input name="ggtype_radio" id="r2c1" value="2串1" type="radio">2串1</label>
                                                                    <label style="display: none;" for="r3c1">
                                                                        <input name="ggtype_radio" id="r3c1" value="3串1" type="radio">3串1</label>
                                                                    <label style="display: none;" for="r3c3">
                                                                        <input name="ggtype_radio" id="r3c3" value="3串3" type="radio">3串3</label>
                                                                    <label style="display: none;" for="r3c4">
                                                                        <input name="ggtype_radio" id="r3c4" value="3串4" type="radio">3串4</label>
                                                                    <label style="display: none;" for="r4c1">
                                                                        <input name="ggtype_radio" id="r4c1" value="4串1" type="radio">4串1</label>
                                                                    <label style="display: none;" for="r4c4">
                                                                        <input name="ggtype_radio" id="r4c4" value="4串4" type="radio">4串4</label>
                                                                    <label style="display: none;" for="r4c5">
                                                                        <input name="ggtype_radio" id="r4c5" value="4串5" type="radio">4串5</label>
                                                                    <label style="display: none;" for="r4c6">
                                                                        <input name="ggtype_radio" id="r4c6" value="4串6" type="radio">4串6</label>
                                                                    <label style="display: none;" for="r4c11">
                                                                        <input name="ggtype_radio" id="r4c11" value="4串11" type="radio">4串11</label>
                                                                    <label style="display: none;" for="r5c1">
                                                                        <input name="ggtype_radio" id="r5c1" value="5串1" type="radio">5串1</label>
                                                                    <label style="display: none;" for="r5c5">
                                                                        <input name="ggtype_radio" id="r5c5" value="5串5" type="radio">5串5</label>
                                                                    <label style="display: none;" for="r5c6">
                                                                        <input name="ggtype_radio" id="r5c6" value="5串6" type="radio">5串6</label>
                                                                    <label style="display: none;" for="r5c10">
                                                                        <input name="ggtype_radio" id="r5c10" value="5串10" type="radio">5串10</label>
                                                                    <label style="display: none;" for="r5c16">
                                                                        <input name="ggtype_radio" id="r5c16" value="5串16" type="radio">5串16</label>
                                                                    <label style="display: none;" for="r5c20">
                                                                        <input name="ggtype_radio" id="r5c20" value="5串20" type="radio">5串20</label>
                                                                    <label style="display: none;" for="r5c26">
                                                                        <input name="ggtype_radio" id="r5c26" value="5串26" type="radio">5串26</label>
                                                                    <label style="display: none;" for="r6c1">
                                                                        <input name="ggtype_radio" id="r6c1" value="6串1" type="radio">6串1</label>
                                                                    <label style="display: none;" for="r6c6">
                                                                        <input name="ggtype_radio" id="r6c6" value="6串6" type="radio">6串6</label>
                                                                    <label style="display: none;" for="r6c7">
                                                                        <input name="ggtype_radio" id="r6c7" value="6串7" type="radio">6串7</label>
                                                                    <label style="display: none;" for="r6c15">
                                                                        <input name="ggtype_radio" id="r6c15" value="6串15" type="radio">6串15</label>
                                                                    <label style="display: none;" for="r6c20">
                                                                        <input name="ggtype_radio" id="r6c20" value="6串20" type="radio">6串20</label>
                                                                    <label style="display: none;" for="r6c22">
                                                                        <input name="ggtype_radio" id="r6c22" value="6串22" type="radio">6串22</label>
                                                                    <label style="display: none;" for="r6c35">
                                                                        <input name="ggtype_radio" id="r6c35" value="6串35" type="radio">6串35</label>
                                                                    <label style="display: none;" for="r6c42">
                                                                        <input name="ggtype_radio" id="r6c42" value="6串42" type="radio">6串42</label>
                                                                    <label style="display: none;" for="r6c50">
                                                                        <input name="ggtype_radio" id="r6c50" value="6串50" type="radio">6串50</label>
                                                                    <label style="display: none;" for="r6c57">
                                                                        <input name="ggtype_radio" id="r6c57" value="6串57" type="radio">6串57</label>
                                                                    <label style="display: none;" for="r7c1">
                                                                        <input name="ggtype_radio" id="r7c1" value="7串1" type="radio">7串1</label>
                                                                    <label style="display: none;" for="r7c7">
                                                                        <input name="ggtype_radio" id="r7c7" value="7串7" type="radio">7串7</label>
                                                                    <label style="display: none;" for="r7c8">
                                                                        <input name="ggtype_radio" id="r7c8" value="7串8" type="radio">7串8</label>
                                                                    <label style="display: none;" for="r7c21">
                                                                        <input name="ggtype_radio" id="r7c21" value="7串21" type="radio">7串21</label>
                                                                    <label style="display: none;" for="r7c35">
                                                                        <input name="ggtype_radio" id="r7c35" value="7串35" type="radio">7串35</label>
                                                                    <label style="display: none;" for="r7c120">
                                                                        <input name="ggtype_radio" id="r7c120" value="7串120" type="radio">7串120</label>
                                                                    <label style="display: none;" for="r8c1">
                                                                        <input name="ggtype_radio" id="r8c1" value="8串1" type="radio">8串1</label>
                                                                    <label style="display: none;" for="r8c8">
                                                                        <input name="ggtype_radio" id="r8c8" value="8串8" type="radio">8串8</label>
                                                                    <label style="display: none;" for="r8c9">
                                                                        <input name="ggtype_radio" id="r8c9" value="8串9" type="radio">8串9</label>
                                                                    <label style="display: none;" for="r8c28">
                                                                        <input name="ggtype_radio" id="r8c28" value="8串28" type="radio">8串28</label>
                                                                    <label style="display: none;" for="r8c56">
                                                                        <input name="ggtype_radio" id="r8c56" value="8串56" type="radio">8串56</label>
                                                                    <label style="display: none;" for="r8c70">
                                                                        <input name="ggtype_radio" id="r8c70" value="8串70" type="radio">8串70</label>
                                                                    <label style="display: none;" for="r8c247">
                                                                        <input name="ggtype_radio" id="r8c247" value="8串247" type="radio">8串247</label>
                                                                    <span class="clear"></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="t04" style="display:none">
                                                            <td>
                                                                3、选择投注倍数
                                                            </td>
                                                        </tr>
                                                        <tr class="t05" style="display:none">
                                                            <td align="left">
                                                                倍数：<input class="jczqtz_input" id="buybs" value="1" size="7" maxlength="5" />(最高99999倍)
                                                            </td>
                                                        </tr>
                                                        <tr class="t04" style="display:none">
                                                            <td>
                                                                4、确认投注结果
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none">
                                                            <td class="td_pad">
                                                                方案注数<span class="r14px" id="showCount">0</span>注，倍数<span class="r14px" id="showBS">1</span>倍，总金额<span
                                                                    class="r14px" id="showMoney">￥0.00</span>元<br />
                                                                方案理论最高奖金：<span id="MaxPrice" class="r14px">￥0.00</span>&nbsp;&nbsp;<a href="javascript:void(0);"
                                                                    id="lookDetails">查看奖金明细</a>
                                                                <div class="touz_btn">
                                                                    <img src="images/btn_dg.gif" id="dgBtn" border="0"
                                                                        style="cursor: pointer;" alt="确认代购"></div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <!--投注结束 -->
            </div>
        </div>
        <input type="hidden" id="jsonggtype" value="{'单关':'A0','2串1':'AA','3串1':'AB','3串3':'AC','3串4':'AD','4串1':'AE','4串4':'AF','4串5':'AG','4串6':'AH','4串11':'AI','5串1':'AJ','5串5':'AK','5串6':'AL','5串10':'AM','5串16':'AN','5串20':'AO','5串26':'AP','6串1':'AQ','6串6':'AR','6串7':'AS','6串15':'AT','6串20':'AU','6串22':'AV','6串35':'AW','6串42':'AX','6串50':'AY','6串57':'AZ', '7串1':'BA', '7串7':'BB', '7串8':'BC', '7串21':'BD', '7串35':'BE', '7串120':'BF', '8串1':'BG', '8串8':'BH', '8串9':'BI', '8串28':'BJ', '8串56':'BK', '8串70':'BL', '8串247':'BM', '8串255':'BN'}" />
        <input type="hidden" id="ggtypename" name="sgtypename" value="普通过关" />
        <input type="hidden" id="ggtypeid" name="ggtypeid" value="AA" />
        <input type="hidden" id="codes" name="codes" />
        <input type="hidden" id="totalmoney" name="totalmoney" />
        <input type="hidden" id="zhushu" name="zhushu" />
        <input type="hidden" id="beishu" name="beishu" />
        <input type="hidden" id="playid" name="playid" value="7303" />
        <input type="hidden" id="playname" name="playname" value="胜分差" />
        <input type="hidden" id="lotid" name="lotid" value="73" />
        <!-- bof 奖金明细弹出窗 -->
        <div class="layer" id="popWindow" style="position: absolute; z-index: 9999; display: none;
            top: 400px; left: 25%;">
            <div class="layer2">
                <div class="lay_box4">
                    <div class="title_t title_t4 clear">
                        <h4>
                            <span id="popWindowTitle">竞彩篮球奖金明细</span></h4>
                        <a href="javascript:void(0)" class="del" title="关闭" onclick="document.getElementById('popWindow').style.display='none'"
                            alt="关闭">关闭</a></div>
                    <div class="lay_content2 scroll" id="popContent">
                        <table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox blank">
                            <tr class="trt">
                                <th>
                                    赛事编号
                                </th>
                                <th class="tb_line">
                                    比赛
                                </th>
                                <th class="tb_line">
                                    您的选择(奖金)
                                </th>
                                <th class="tb_line">
                                    最小奖金
                                </th>
                                <th class="tb_line">
                                    最大奖金
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    周四026
                                </td>
                                <td>
                                    德甲
                                </td>
                                <td>
                                    胜(1.76) 平(3.07) 负(9.28)
                                </td>
                                <td class="vcenter">
                                    14.21
                                </td>
                                <td class="td_line vcenter">
                                    112.00
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" class="td_line">
                                    过关方式：<span class="red">9串1</span> 倍数：<span class="red">1</span>倍 方案总金额：<span class="red">￥2.00</span>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox">
                            <tr class="trt">
                                <th rowspan="2">
                                    命中场数
                                </th>
                                <th class="tb_line tb_line2">
                                    中奖注数
                                </th>
                                <th rowspan="2" class="tb_line">
                                    倍数
                                </th>
                                <th colspan="2" class="tb_line tb_line2">
                                    奖金范围
                                </th>
                            </tr>
                            <tr class="trt">
                                <th class="tb_line">
                                    5串1
                                </th>
                                <th class="tb_line">
                                    最小奖金
                                </th>
                                <th class="tb_line">
                                    最大奖金
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    周四026
                                </td>
                                <td>
                                    1注
                                </td>
                                <td>
                                    1234
                                </td>
                                <td>
                                    4.21元[明细]
                                </td>
                                <td class="td_line vcenter">
                                    4.21元[明细]
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    注：奖金预测sp值为投注时即时sp值，最终奖金以开奖sp值为准<span class="red"></span>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellspacing="0" cellpadding="0" class="yuce_tablebox">
                            <tr class="trt">
                                <th>
                                    过关方式
                                </th>
                                <th class="tb_line">
                                    中奖注数
                                </th>
                                <th class="tb_line">
                                    中12场 最大奖金 中奖明细
                                </th>
                                <th class="tb_line">
                                    奖金
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    9串1
                                </td>
                                <td>
                                    9565注
                                </td>
                                <td>
                                    <p class="td_width">
                                        [50]9.38×[46]4.27×[53]4.18×[51]3.29×[43]3.16×[54]3.09×1倍×2元×65%= 6991.85元</p>
                                </td>
                                <td class="td_line vcenter">
                                    13243.11元
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    合计
                                </td>
                                <td>
                                    23注
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="td_line vcenter">
                                    <span class="bold">131.00</span>元
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <!-- eof 奖金明细弹出窗 -->
        <asp:HiddenField runat="server" ID="HidNowTime"></asp:HiddenField>
    </div>
    </form>
</body>

<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="JScript/tool.js"></script>
<script type="text/javascript" src="JScript/com.js"></script>
<script type="text/javascript" src="JScriptClient/gg_sfc.js"></script>
<script type="text/javascript" src="JScriptClient/client.js"></script>
<script id="config" type="text/javascript" src="JScript/form.js?1"></script>

</html>
