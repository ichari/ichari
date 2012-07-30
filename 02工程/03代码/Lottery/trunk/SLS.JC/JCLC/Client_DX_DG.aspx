<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Client_DX_DG.aspx.cs" Inherits="JCLC_Client_DX_DG" EnableViewState="false"%>
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
    <form id="buy_form" runat="server" ajax="Buy_Dg_Handler.ashx">
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
                <div style="position: absolute; left: 530px; top: 230px;" style="display:none">
                </div>
                <!-- 开始对阵列表 !-->
                    <table class="ttd showodds" id="table_vs" width="100%" border="0" cellpadding="0" cellspacing="1">
				<tbody>
					<tr class="gg_form_tr_01_lq">
						<td rowspan="2" style="width:90px;">赛事编号</td>
						<td rowspan="2" style="width:100px;">赛事类型</td>
						<td rowspan="2" style="width:80px;">截止时间</td>
						<td rowspan="2">客队</td>
						<td rowspan="2" style="width:80px;">预设总分</td>
						<td rowspan="2">主队</td>
						<td colspan="3">请选择投注</td>
						
					</tr>
					<tr class="gg_form_tr_02_lq">
						<td width="80" class="s">大分</td>
						<td width="80" class="s">小分</td>
						<td width="40">全包</td>
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
										<td class="jczqtouzu_title_gg_lq"><span class="bold">您选择的场次及投注选项</span></td>
									</tr>
									<tr>
										<td>
											<table class="table_bdtz_gg_lq" width="100%" border="0" cellpadding="4" cellspacing="0">
												<tbody>
													<tr class="t02">
														<td width="100" align="center">赛事编号</td>
														<td align="center">客队 VS 主队</td>
														<td align="center" class="bnone">您的投注选项</td>
													</tr>
												</tbody>
												<tbody id="row_tpl" style="display:none">
													<tr class="t03"><td><input checked="checked" value="" name="" type="checkbox"><span></span></td><td align="center"></td><td align="center" class="bnone"><label style="display: none;"><input checked="checked" value="1" type="checkbox">大分</label><label style="display: none;"><input checked="checked" value="2" type="checkbox">小分</label></td></tr>
												</tbody>
												<tbody id="selectTeams"></tbody>
											</table>
										</td>
									</tr>
									<tr>
										<td align="center" height="50">
											<span style="line-height:22px; cursor:pointer" id="clearAllSelect"><img alt="清空投注" src="images/b_img_014.gif" /></span>
										</td>
									</tr>
								</tbody>
							</table>
						</td>
 
						<td valign="top">
							<table class="table_jczqtz_gg_lq" style="" width="100%" border="0" cellpadding="4" cellspacing="0">
								<tbody>
									<tr>
										<td  class="jczqtouzu_title_gg_lq">请选择过关方式</td>
									</tr>
									<tr>
										<td>
											<table class="table_bdtz_gg_lq" width="100%" border="0" cellpadding="4" cellspacing="0" style="margin:0">
												<tbody>
													<tr class="t04" style="display:none">
														<td>1、选择过关类型</td>
													</tr>
													<tr class="t05">
														<td>
															<input name="ssq_dg" checked="checked" type="radio" />单关
															(您选择了<span id="selectMatchNum" style="color:#F00">0</span>场比赛单关投注)
														</td>
													</tr>
 
													<tr class="t04" style="display:none">
														<td>2、选择投注倍数</td>
													</tr>
													<tr class="t05" style="display:none">
														<td align="left">倍数：<input class="jczqtz_input" id="buybs" value="1" size="7" maxlength="5" />(最高99999倍)</td>
													</tr>
													<tr class="t04" style="display:none">
														<td>3、确认投注结果</td>
													</tr>
													<tr style="display:none">
														<td class="td_pad">
															方案注数<span class="r14px" id="showCount">0</span>注，倍数<span class="r14px" id="showBS">1</span>倍，总金额<span class="r14px" id="showMoney">￥0.00</span>元
															<div class="touz_btn"><img src="images/btn_dg.gif" id="dgBtn" border="0" style="cursor: pointer;" alt="确认代购"></div>
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
        <input type="hidden" id="ggtypeid" name="ggtypeid" value="A0" />
        <input type="hidden" id="codes" name="codes" />
        <input type="hidden" id="totalmoney" name="totalmoney" />
        <input type="hidden" id="zhushu" name="zhushu" />
        <input type="hidden" id="beishu" name="beishu" />
        <input type="hidden" id="playid" name="playid" value="7304" />
        <input type="hidden" id="playname" name="playname" value="大小分" />
        <input type="hidden" id="lotid" name="lotid" value="73" />
        <asp:HiddenField runat="server" ID="HidNowTime"></asp:HiddenField>
    </div>
    </form>
</body>

<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="JScript/tool.js"></script>
<script type="text/javascript" src="JScript/com.js"></script>
<script type="text/javascript" src="JScriptClient/dg_1.js"></script>
<script type="text/javascript" src="JScriptClient/client.js"></script>
<script id="config" type="text/javascript" src="JScript/form.js?1"></script>


</html>