<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Client_SPF_DG.aspx.cs" Inherits="JCZC_Client_SPF_DG" EnableViewState="false"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>让球胜平负-竞彩足球</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="Style/tips_globle.css" rel="stylesheet" type="text/css" />
    <link href="Style/public.css" rel="stylesheet" type="text/css" />
    <link href="Style/trade.css" rel="stylesheet" type="text/css" />
    <link href="/Style/pub.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="buy_form" runat="server" ajax="Buy_Dg_Handler.ashx">
    
    <div style="width:814px;">
        <div style="width:814px;">
            <!--投注开始 -->
            <!-- 开始选择 !-->
            <div class="touzhu" style="border-top-color: #FFF;">
                <div class="reply_type_01" style="display:none">
                    <p class="time_scroll">
                        <span class="time_scroll2" id="TimeDisplay"></span>
                    </p>
                    <img src="Images/btn_sai.gif" style="cursor: pointer;"
                        onclick="document.getElementById('leagueBox').style.display='block'" align="absmiddle"
                        alt="赛事选择">
                    已隐藏<span class="red" id="hideCount">0</span>场比赛 <a href="javascript:void 0" id="showAllTeam">
                        <font color="#ff0000">恢复</font></a> |
                      <input name="rq" checked="checked" id="showRq" type="checkbox" />
                      <asp:Label ID="lbShowRq" runat="server"></asp:Label>
                      <input name="norq" checked="checked" id="showNoRq" type="checkbox" />
                      <asp:Label ID="lbShowRoRaq" runat="server"></asp:Label>
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
                                    <img src="Images/select_all.gif" alt="全选" style="cursor: pointer;"
                                        id="selectAllBtn" /><img src="Images/select_alt.gif"
                                            alt="反选" style="cursor: pointer;" id="selectOppBtn" /><img src="Images/close2.gif"
                                                alt="关闭" style="cursor: pointer;" onclick="document.getElementById('leagueBox').style.display='none'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- 日期选择开始 -->
                <div style="position: absolute; left: 530px; top: 230px;">
                </div>
                  <!-- 开始对阵列表 !-->
                   <table class="ttd showodds" id="table_vs" width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tbody><tr class="gg_form_tr_01">
                  <td rowspan="2">赛事编号</td>
                  <td rowspan="2" width="62">赛事<br />类型 </td>
                  <td rowspan="2">截止<br />时间</td>
                  <td rowspan="2">主队</td>
                  <td rowspan="2">让球</td>
                  <td rowspan="2">客队</td>
                   <td colspan="3">
                    <select id="op_col">
                      <option value="99家平均">99家平均欧指</option>
                      <option value="威廉希尔">威廉希尔</option>
                      <option value="立博">立博</option>
                      <option value="Bet365">Bet365</option>
                      <option value="澳门彩票">澳门彩票</option>
                    </select></td>
                  <td rowspan="2">数据</td>
                  <td colspan="4">请选择投注</td>
                  </tr>
                  <tr class="gg_form_tr_02">
                  <td height="18">主胜</td>
                  <td>平局</td>
                  <td>主负</td>
                  <td>胜(3)</td>
                  <td>平(1)</td>
                  <td>负(0)</td>
                  <td>全包</td>
                  </tr>
                 <%= MatchList %>
                 </tbody>
            </table>
            <!-- start 合买区  !-->
  <table class="bd_touzhu blank" width="99%" border="0" cellpadding="0" cellspacing="0">
  <tbody><tr>
  <td class="l" valign="top" width="58%">
  <table class="table_jczqtz_gg" width="98%" border="0" cellpadding="4" cellspacing="0">
  <tbody><tr>
  <td class="jczqtouzu_title_gg"><span class="bold">您选择的场次及投注选项</span></td>
  </tr>
  <tr>
  <td><table class="table_bdtz_gg" width="100%" border="0" cellpadding="4" cellspacing="0">
    <tbody><tr class="t02">
      <td width="100" align="center">赛事编号</td>
      <td align="center">比赛对阵</td>
      <td align="center" class="bnone">您的投注选项</td>
    </tr>
      </tbody>
      <tbody id="row_tpl" style="display:none;">
      <tr class="t03"><td><input checked="checked" value="2929" name="m2929" type="checkbox"><span>周四006</span></td><td align="center">巴兰基利亚青年 VS 蒙特维迪奥竞技</td><td align="center" class="bnone"><label style="display: none;"><input checked="checked" value="3" type="checkbox">胜</label><label style="display: none;"><input checked="checked" value="1" type="checkbox">平</label><label style="display: none;"><input checked="checked" value="0" type="checkbox">负</label></td></tr>
      </tbody>
      <tbody id="selectTeams"></tbody>
  </table></td>
  </tr>
  <tr>
  <td align="center" height="50"><span style="line-height: 22px; cursor: pointer;" id="clearAllSelect"><img src="Images/b_img_014.gif" title="清空投注"></span></td>
  </tr>
  </tbody></table>
  </td>
  <td class="r" valign="top"><table class="table_jczqtz_gg" style="" width="100%" border="0" cellpadding="4" cellspacing="0">
     <tbody><tr>
  <td colspan="5" class="jczqtouzu_title_gg">请选择过关方式</td>
  </tr>
      <tr>
        <td><table class="table_bdtz_gg" width="100%" border="0" cellpadding="4" cellspacing="0" style="margin:0;">
          <tbody><tr class="t04" style="display:none">
            <td>1、过关方式</td>
            </tr>
          <tr class="t05">
            <td><input name="ssq_dg" checked="checked" type="radio">单关(您选择了<span style="color:#F00" id="selectMatchNum">0</span>场比赛单关投注)</td>
            </tr>
          <tr class="t04" style="display:none">
                <td>
                    2、确认投注信息
                </td>
            </tr>
            <tr style="display:none">
                <td>
                    <table class="table_BetInfo" width="100%" border="0" cellpadding="2" cellspacing="0" style="display:none">
                        <tr>
                            <td width="80" align="right">
                                方案金额：
                            </td>
                            <td>
                                方案注数<span class="r14px" id="showCount">0</span>注，<span
                                    class="r14px" id="showMoney">￥0.00</span>元
                            </td>
                        </tr>
                        <tr>
                            <td width="80" align="right">
                                投注倍数：
                            </td>
                            <td align="left">
                                <input class="jczqtz_input" id="buybs" value="1" size="7" maxlength="5" />
                                (最高99999倍)
                            </td>
                        </tr>
                        <tr>
                            <td width="80" align="right">
                                购彩类型：
                            </td>
                            <td align="left">
                                <input name="Scheme_Type" checked="checked" type="radio" id="Scheme_Buy" />代购<input
                                    name="Scheme_Type" type="radio" id="Scheme_join" />合买
                            </td>
                        </tr>
                        <tbody id="trShowJion" style="height: 36px; text-align: center; padding-left: 10px; padding-right: 10px; display:none;">
                            <tr>
                                <td>
                                    佣金比率
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_SchemeBonusScale" name="tb_SchemeBonusScale" style="width: 56px;" value="4"
                                        maxlength="10" />% &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;佣金比例只能为0~10。
                                </td>
                            </tr>
                            <tr id="trShare">
                                <td>
                                    我要分成
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_Share" name="tb_Share" style="width: 56px;" maxlength="10" value="1" />份，每份
                                    <span id="lab_ShareMoney" style="color: Red">0.00</span>&nbsp;元。&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    我要认购
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_BuyShare" name="tb_BuyShare" style="width: 56px;" value="1" />份，金额 <span id="lab_BuyMoney"
                                            style="color: Red">0.00</span>&nbsp;元。
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    我要保底
                                </td>
                                <td align="left">
                                    <input type="text" id="tb_AssureShare" name="tb_AssureShare" style="width: 56px;" value="0">份，保底 <span id="lab_AssureMoney"
                                            style="color: Red">0.00</span>&nbsp;元。&nbsp;
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
                        </tbody>
                        <tr>
                            <td width="80" align="right">
                                保密设置：
                            </td>
                            <td align="left" id="ggSecrecyLevel">
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel0" value="0" checked="checked" /><span>不保密</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel1" value="1" /><span>保密到截止</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel2" value="2" /><span>保密到开奖</span>
                                <input type="radio" name="SecrecyLevel" id="SecrecyLevel3" value="3" /><span>永久保密</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="touz_btn"><img src="Images/btn_dg.gif" id="dgBtn" border="0" style="cursor: pointer;" alt="确认代购">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src="Images/FilterShrink.gif" id="FilterBtn" border="0" style="cursor: pointer;" alt="高级过滤" /></div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody></table>
  </td>
      </tr>
    </tbody></table></td>
  </tr>
  </tbody>
  </table>
            <!--投注结束 -->
        </div>
    </div>
<input type="hidden" id="jsonggtype" value="{'单关':'A0','2串1':'AA','3串1':'AB','3串3':'AC','3串4':'AD','4串1':'AE','4串4':'AF','4串5':'AG','4串6':'AH','4串11':'AI','5串1':'AJ','5串5':'AK','5串6':'AL','5串10':'AM','5串16':'AN','5串20':'AO','5串26':'AP','6串1':'AQ','6串6':'AR','6串7':'AS','6串15':'AT','6串20':'AU','6串22':'AV','6串35':'AW','6串42':'AX','6串50':'AY','6串57':'AZ'}" />
<input type="hidden" id="ggtypename" name="sgtypename" value="普通过关" />
<input type="hidden" id="ggtypeid" name="ggtypeid" value="A0" />
<input type="hidden" id="codes" name="codes" />
<input type="hidden" id="totalmoney" name="totalmoney" />
<input type="hidden" id="zhushu" name="zhushu" />
<input type="hidden" id="beishu" name="beishu" />
<input type="hidden" id="playid" name="playid" value="7201" />
<input type="hidden" id="playname" name="playname" value="竞彩胜平负" />
<input type="hidden" id="lotid" name="lotid" value="72" />
<input type="hidden" id="SecrecyL" name="SecrecyL" value="0" />
<input type="hidden" id="lotType" name="lotType" value="spf" />
<span id="spnUnsureDan" style="display:none;"><select name="dpUnsureDan" id="Select1" ></select></span>
</div>

<asp:HiddenField runat="server" ID="HidNowTime"></asp:HiddenField>
</form>
</body>

<script src="/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
<script type="text/javascript" src="JScript/tool.js"></script>
<script type="text/javascript" src="JScript/com.js"></script>
<script type="text/javascript" src="JScriptClient/dg.js"></script>
<script type="text/javascript" src="JScriptClient/client.js"></script>
<script id="config" type="text/javascript" src="JScript/form.js?1"></script>

</html>