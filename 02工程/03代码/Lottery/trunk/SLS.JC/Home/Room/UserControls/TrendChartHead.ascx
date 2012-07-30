<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TrendChartHead.ascx.cs" Inherits="Home_Room_UserControls_TrendChartHead" %>
<div>
    <table width="960" cellpadding="0" cellspacing="0" style="background-image: url('/Home/Room/images/cz_blue_bg.jpg');text-align: center; cursor: pointer;line-height: 20px; font-weight: bold; color: White;border: 1px solid #9BBFCA;margin:0 auto 0 auto;">
        <tbody id="tbLotteries" style="color:#226699">
            <tr>
                <td class="blue12" id='Loottery_5' onclick='mChangeLottery(5);' style="width:85px;">
                    双色球
                </td>
                <td class="blue12" id='Loottery_13' onclick='mChangeLottery(13);' style="width:85px;">
                    七乐彩
                </td>
                <td class="blue12" id='Loottery_39' onclick='mChangeLottery(39);' style="width:85px;">
                    超级大乐透
                </td>
                <td class="blue12" id='Loottery_29' onclick='mChangeLottery(29);' style="width:85px;">
                    时时乐
                </td>
                <td class="blue12" id='Loottery_6' onclick='mChangeLottery(6);' style="width:85px;">
                    福彩3D
                </td>
                <td class="blue12" id='Loottery_61'onclick='mChangeLottery(61);' style="width:85px;">
                    时时彩
                </td>
                <td class="blue12" id='Loottery_63' onclick='mChangeLottery(63);' style="width:85px;">
                    排列3
                </td>
                <td class="blue12" id='Loottery_64' onclick='mChangeLottery(64);' style="width:85px;">
                    排列5
                </td>
                <td class="blue12" id='Loottery_3' onclick='mChangeLottery(3);' style="width:85px;">
                    七星彩
                </td>
                <td class="blue12" id='Loottery_9' onclick='mChangeLottery(9);' style="width:85px;">
                    22选5
                </td>
                <%--<td class="blue12" id='Loottery_62'onclick='mChangeLottery(62);' style="width:85px;">
                    十一运夺金
                </td>--%>
                <td class="blue12" id='Loottery_28'onclick='mChangeLottery(28);' style="width:85px;">
                    重庆时时彩
                </td>
            </tr>
        </tbody>
    </table>
    <table id="Charts" cellpadding="3" cellspacing="0" style="margin:0 auto 0 auto;width:960px;">
        <tbody id="tbCharts" class="black12" style="line-height: 25px;">
            <tr>
                <td id='Charts5' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>双色球:</span><a
                        href='../SSQ/SSQ_CGXMB.aspx' target="mainFrame">常规项目表走势图</a> | <a href='../SSQ/SSQ_ZHFB.aspx'
                            target="mainFrame">综合分布走势图</a> | <a href='../SSQ/SSQ_3FQ.aspx' target="mainFrame">3分区分布走势图</a>
                    | <a href='../SSQ/SSQ_DX.aspx' target="mainFrame">大小分析走势图</a> | <a href='../SSQ/SSQ_JO.aspx'
                        target="mainFrame">奇偶分析走势图</a> | <a href='../SSQ/SSQ_ZH.aspx' target="mainFrame">质合分析走势图</a>
                    | <a href='../SSQ/SSQ_HL.aspx' target="mainFrame">红蓝球走势图</a> | <a href='../SSQ/SSQ_BQZST.aspx'
                        target="mainFrame">蓝球综合走势图</a>
                </td>
                <td id='Charts6' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-right: 8px;'>福彩3D:</span><a href='../FC3D/FC3D_ZHXT.aspx'
                        target="mainFrame">综合分布遗漏走势图</a> | <a href='../FC3D/FC3D_C3YS.aspx' target="mainFrame">除三余数形态遗漏走势图</a>
                    | <a href='../FC3D/FC3D_ZHZST.aspx' target="mainFrame">质合形态遗漏走势图</a> | <a href='../FC3D/FC3D_XTZST.aspx'
                        target="mainFrame">形态走势遗漏走势图</a> | <a href='../FC3D/FC3D_KDZST.aspx' target="mainFrame">跨度走势遗漏走势图</a>
                    | <a href='../FC3D/FC3D_HZZST.aspx' target="mainFrame">和值走势遗漏走势图</a> | <a href='../FC3D/FC3D_DZXZST.aspx'
                        target="mainFrame">大中小形态遗漏走势图</a> | <a href='../FC3D/FC3D_C3YS_HTML.aspx' target="mainFrame">除三余数号码分区表走势图</a>
                    | <a href='../FC3D/FC3D_2CHW.aspx' target="mainFrame">二次和尾差尾查询表走势图</a> | <a href='../FC3D/FC3D_DSHM.aspx'
                        target="mainFrame">单双点号码分区表走势图</a>
                    <a href='../FC3D/FC3D_DTHM.aspx' target="mainFrame">胆托号码分区表走势图</a> | <a href='../FC3D/FC3D_DX_JO.aspx'
                        target="mainFrame">大小—奇偶号码分区表走势图</a> | <a href='../FC3D/FC3D_HSWH.aspx' target="mainFrame">和数尾号码分区表走势图</a>
                    | <a href='../FC3D/FC3D_HSZ.aspx' target="mainFrame">和数值号码分区表走势图</a> | <a href='../FC3D/FC3D_KDZH.aspx'
                        target="mainFrame">跨度组合分区表走势图</a> | <a href='../FC3D/FC3D_JO_DX.aspx' target="mainFrame">奇偶—大小号码分区表走势图</a>
                    | <a href='../FC3D/FC3D_LHZH.aspx' target="mainFrame">连号组合分区表走势图</a> | <a href='../FC3D/FC3D_ZS.aspx'
                        target="mainFrame">质数号码分区表走势图</a>
                </td>
                <td id='Charts3' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>七星彩:</span><a
                        href='7X_HMFB.aspx' target='mainFrame'>分布走势图</a>
                    | <a href='7X_CF.aspx' target='mainFrame'>重复分布走势图</a>
                    | <a href='7X_LH.aspx' target='mainFrame'>连号分布走势图</a>
                    | <a href='7X_DX.aspx' target='mainFrame'>大小分布走势图</a>
                    | <a href='7X_JO.aspx' target='mainFrame'>奇偶分布走势图</a>
                    | <a href='7X_YS.aspx' target='mainFrame'>余数分布走势图</a>
                    | <a href='7X_HZheng.aspx' target='mainFrame'>
                        和值分布走势图</a> | <a href='X7_012.aspx' target='mainFrame'>
                            012路分布走势图</a> | <a href='7X_ZH.aspx' target='mainFrame'>
                                质合分布走势图</a> | <a href='X7_DZX.aspx' target='mainFrame'>
                                    大中小分布走势图</a>
                </td>
                <td id='Charts61' style="text-align: left;">
                    <span class='blue14' style='padding-right: 8px;'>时时彩:</span><a href='../JXSSC/SSC_5X_ZHFB.aspx' target="mainFrame">标准五星综合走势图</a> 
                    | <a href='../JXSSC/SSC_5X_ZST.aspx' target="mainFrame">标准五星走势图</a>| <a href='../JXSSC/SSC_5X_HZZST.aspx' target="mainFrame">五星和值走势图</a> 
                    | <a href='../JXSSC/SSC_5X_KDZST.aspx' target="mainFrame">五星跨度走势图</a> | <a href='../JXSSC/SSC_5X_PJZZST.aspx' target="mainFrame">五星平均值走势图</a>
                    | <a href='../JXSSC/SSC_5X_DXZST.aspx' target="mainFrame">五星大小走势图</a> | <a href='../JXSSC/SSC_5X_JOZST.aspx' target="mainFrame">五星奇偶走势图</a> 
                    | <a href='../JXSSC/SSC_5X_ZHZST.aspx' target="mainFrame""'">五星质合走势图</a> | <a href='../JXSSC/SSC_4X_ZHFB.aspx' target="mainFrame">标准四星综合走势图</a> 
                    | <a href='../JXSSC/SSC_4X_ZST.aspx' target="mainFrame">标准四星走势图</a> | <a href='../JXSSC/SSC_4X_HZZST.aspx' target="mainFrame">四星和值走势图</a>
                    | <a href='../JXSSC/SSC_4X_KDZST.aspx' target="mainFrame">四星跨度走势图</a> | <a href='../JXSSC/SSC_4X_PJZZST.aspx' target="mainFrame">四星平均值走势图</a> 
                    | <a href='../JXSSC/SSC_4X_DXZST.aspx' target="mainFrame">四星大小走势图</a> | <a href='../JXSSC/SSC_4X_JOZST.aspx' target="mainFrame">四星奇偶走势图</a> 
                    | <a href='../JXSSC/SSC_4X_ZHZST.aspx' target="mainFrame">四星质合走势图</a> | <a href='../JXSSC/SSC_3X_ZHZST.aspx' target="mainFrame">标准三星综合</a>
                    | <a href='../JXSSC/SSC_3X_ZST.aspx' target="mainFrame">标准三星走势图</a> | <a href='../JXSSC/SSC_3X_HZZST.aspx' target="mainFrame">三星和值走势图</a> 
                    | <a href='../JXSSC/SSC_3X_HZWZST.aspx' target="mainFrame">三星和值尾走势图</a> | <a href='../JXSSC/SSC_3X_KDZST.aspx' target="mainFrame">三星跨度走势图</a> 
                    | <a href='../JXSSC/SSC_3X_DXZST.aspx' target="mainFrame">三星大小走势图</a> | <a href='../JXSSC/SSC_3X_JOZST.aspx' target="mainFrame">三星奇偶走势图</a>
                    | <a href='../JXSSC/SSC_3X_ZhiHeZST.aspx' target="mainFrame">三星质合走势图</a> | <a href='../JXSSC/SSC_3X_DX_012_ZST.aspx'target="mainFrame">单选012路走势图</a> 
                    | <a href='../JXSSC/SSC_3X_ZX_012_ZST.aspx' target="mainFrame">组选012路走势图</a> | <a href='../JXSSC/SSC_2X_ZHFBZST.aspx' target="mainFrame">标准二星综合走势图</a>
                    | <a href='../JXSSC/SSC_2X_HZZST.aspx' target="mainFrame">二星和值走势图</a> | <a href='../JXSSC/SSC_2X_HZWZST.aspx' target="mainFrame">二星和尾走势图</a> 
                    | <a href='../JXSSC/SSC_2XPJZZST.aspx' target="mainFrame">二星均值走势图</a> | <a href='../JXSSC/SSC_2X_KDZST.aspx' target="mainFrame">二星跨度走势图</a> 
                    | <a href='../JXSSC/SSC_2X_012ZST.aspx' target="mainFrame">二星012路走势图</a> | <a href='../JXSSC/SSC_2X_MaxZST.aspx' target="mainFrame">二星最大值走势图</a>
                    | <a href='../JXSSC/SSC_2X_MinZST.aspx' target="mainFrame">二星最小值走势图</a> | <a href='../JXSSC/SSC_2X_DXDSZST.aspx' target="mainFrame">大小单双走势图</a>
                </td>
                <td id='Charts9' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>22选5:</span><a
                        href='22X5_HMFB.aspx' target='mainFrame'>
                        分布走势图</a> | <a href='22X5_LH.aspx' target='mainFrame'>
                            连号分布走势图</a> | <a href='22X5_JO.aspx' target='mainFrame'>
                                奇偶分布走势图</a> | <a href='22X5_WeiHaoCF.aspx'
                                    target='mainFrame'>重复分布走势图</a> | <a href='22X5_HMLR.aspx'
                                        target='mainFrame'>号码历史冷热走势图</a> | <a href='22X5_WeiHao.aspx'
                                            target='mainFrame'>尾号分布走势图</a> | <a href='22X5_YS.aspx'
                                                target='mainFrame'>余数分布走势图</a> | <a href='22X5_HZ_Zong.aspx'
                                                    target='mainFrame'>和值分布走势图</a>
                </td>
                <td id='Charts39' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>超级大乐透:</span><a
                        href='../CJDLT/CJDLT_HMFB.aspx' target='mainFrame'>
                        分布走势图</a> | <a href='CJDLT_jima.aspx' target='mainFrame'>
                            前区走势图</a> | <a href='CJDLT_TeMa.aspx' target='mainFrame'>
                                后区走势图</a> | <a href='CJDLT_JimaLengRe.aspx'
                                    target='mainFrame'>前区冷热走势图</a> | <a href='CJDLT_TemaLengRe.aspx'
                                        target='mainFrame'>后区冷热走势图</a> | <a href='CJDLT_JO.aspx'
                                            target='mainFrame'>奇偶走势图</a> | <a href='CJDLT_YS.aspx'
                                                target='mainFrame'>余数走势图</a> | <a href='CJDLT_JiMaWeihao.aspx'
                                                    target='mainFrame'>前区尾号分布走势图</a> | <a href='CJDLT_HZZong.aspx'
                                                        target='mainFrame'>和值分析走势图</a> | <a href='CJDLT_LH.aspx'
                                                            target='mainFrame'>连号分析走势图</a>
                </td>
                <td id='Charts63' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>排列3:</span><a
                        href='PL3_HMFB.aspx' target='mainFrame'> 排列三分布走势图
                    </a>| <a href='PL3_CF.aspx' target='mainFrame'>
                        排列三重复分布走势图</a> | <a href='PL3_LH.aspx' target='mainFrame'>
                            排列三连号分布走势图</a> | <a href='PL3_DX.aspx' target='mainFrame'>
                                排列三大小分布走势图</a> | <a href='PL3_JO.aspx' target='mainFrame'>
                                    排列三奇偶分布走势图</a> | <a href='PL3_YS.aspx' target='mainFrame'>
                                        排列三余数分布走势图</a> | <a href='PL3_HZ.aspx' target='mainFrame'>
                                            排列三和值分布走势图</a> | <a href='PL3_012.aspx' target='mainFrame'>
                                                排列三012路分布走势图</a> | <a href='PL3_KD.aspx' target='mainFrame'>
                                                    排列三跨度分布走势图</a> | <a href='PL3_ZX.aspx' target='mainFrame'>
                                                        排列三组选分布走势图</a> | <a href='PL3_ZH.aspx' target='mainFrame'>
                                                            排列三质合数分布走势图</a> | <a href='PL3_DZX.aspx' target='mainFrame'>
                                                                排列三大中小分布走势图</a> | <a href='PL3_WH.aspx' target='mainFrame'>
                                                                    排列三位和分布走势图 </a>
                </td>
                <td id='Charts64' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>排列5:</span><a
                        href='PL5_KJFB.aspx' target='mainFrame'> 排列五开奖分布走势图</a>
                    | <a href='PL5_CF.aspx' target='mainFrame'>排列五重复分布走势图</a>
                    | <a href='PL5_LH.aspx' target='mainFrame'>排列五连号分布走势图</a>
                    | <a href='PL5_DX.aspx' target='mainFrame'>排列五大小分布走势图</a>
                    | <a href='PL5_JO.aspx' target='mainFrame'>排列五奇偶分布走势图</a>
                    | <a href='PL5_YS.aspx' target='mainFrame'>排列五余数分布走势图</a>
                    | <a href='PL5_HZ.aspx' target='mainFrame'>排列五和值分布走势图</a>
                    | <a href='PL5_012.aspx' target='mainFrame'>排列五大中小分布走势图</a>
                </td>
                <td id='Charts62' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>十一运夺金:</span><a
                        class="link_b" href="../SYYDJ/SYDJ_FBZS.aspx" onclick="setAColor(this);" target="mainFrame">分布走势图</a>
                    | <a href="../SYYDJ/SYDJ_DWZS.aspx" onclick="setAColor(this);" target="mainFrame">定位走势图</a>
                    | <a href="../SYYDJ/SYDJ_HZFB.aspx" onclick="setAColor(this);" target="mainFrame">和值分布走势图</a>
                    | <a href="../SYYDJ/SYDJ_Q2FBT.aspx" onclick="setAColor(this);" target="mainFrame">前二分布走势图</a>
                    | <a href="../SYYDJ/SYDJ_Q2ZXDYB.aspx" onclick="setAColor(this);" target="mainFrame">前二组选对应表走势图</a>
                    | <a href="../SYYDJ/SYDJ_Q2HZ.aspx" onclick="setAColor(this);" target="mainFrame">前二和值走势图</a>
                    | <a href="../SYYDJ/SYDJ_Q3FWT.aspx" onclick="setAColor(this);" target="mainFrame">前三分位图走势图</a>
                    | <a href="../SYYDJ/SYDJ_Q3FBT.aspx" onclick="setAColor(this);" target="mainFrame">前三分布走势图</a>
                    | <a href="../SYYDJ/SYDJ_Q3HZT.aspx" onclick="setAColor(this);" target="mainFrame">前三和值图走势图</a>
                </td>
                <td id='Charts29' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>时时乐:</span><a
                        href='../SHSSL/SHSSL_ZHFB.aspx' target="mainFrame">综合分布图（基本走势图）</a> | <a href='../SHSSL/SHSSL_012.aspx'
                            target="mainFrame">012路（除3余数）走势图</a> | <a href='../SHSSL/SHSSL_DX.aspx' target="mainFrame">大小分析走势图</a>
                    | <a href='../SHSSL/SHSSL_JO.aspx' target="mainFrame">奇偶分析走势图</a> | <a href='../SHSSL/SHSSL_ZH.aspx'
                        target="mainFrame">质合分析走势图</a> | <a href='../SHSSL/SHSSL_HZ.aspx' target="mainFrame">和值走势图</a>
                </td>
                <td id='Charts13' style="padding-left: 30px; text-align: left;">
                    <span class='blue14' style='padding-left: 30px; padding-right: 8px;'>七乐彩:</span><a
                        href='/TrendCharts/QLC/7LC_CGXMB.aspx'  target="mainFrame">七乐彩常规项目表走势图</a> | <a href='/TrendCharts/QLC/7LC_ZHFB.aspx'
                                target="mainFrame">七乐彩综合分布走势图</a>
                </td>
                <td id='Charts28' style="text-align: left;">
                    <span class='blue14' style='padding-right: 8px;'>重庆时时彩:</span><a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">五星基本走势图</a> 
                    | <a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">五星和值跨度</a>
                    | <a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">三星基本走势</a> | <a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">三星和值跨度</a> 
                    | <a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">三星大小奇偶</a>
                    | <a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">二星基本走势</a> | <a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">二星和值跨度</a> 
                    | <a href='/TrendCharts/CQSSC/SSC_5X_ZST.aspx' target="mainFrame">大小单双走势</a>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<script type="text/javascript" language="javascript">
    var lastClickTabMenu = null;
    //特显样式
    if (location.href.indexOf("/SSQ/") >= 0) {
        mClick(document.getElementById('Loottery_5'), 'Charts5');
    }
    else if (location.href.indexOf("/CJDLT/") >= 0) {
        mClick(document.getElementById('Loottery_39'), 'Charts39');
    }
    else if (location.href.indexOf("/SYYDJ/") >= 0) {
        mClick(document.getElementById('Loottery_62'), 'Charts62');
    }
    else if (location.href.indexOf("/SHSSL/") >= 0) {
        mClick(document.getElementById('Loottery_29'), 'Charts29');
    }
    else if (location.href.indexOf("/FC3D/") >= 0) {
        mClick(document.getElementById('Loottery_6'), 'Charts6');
    }
    else if (location.href.indexOf("/QLC/") >= 0) {
        mClick(document.getElementById('Loottery_13'), 'Charts13');
    }
    else if (location.href.indexOf("/JXSSC/") >= 0) {
        mClick(document.getElementById('Loottery_61'), 'Charts61');
    }
    else if (location.href.indexOf("/FC3D/") >= 0) {
        mClick(document.getElementById('Loottery_6'), 'Charts6');
    }
    else if (location.href.indexOf("/PL3/") >= 0) {
        mClick(document.getElementById('Loottery_63'), 'Charts63');
    }
    else if (location.href.indexOf("/PL5/") >= 0) {
        mClick(document.getElementById('Loottery_64'), 'Charts64');
    }
    else if (location.href.indexOf("/7Xing/") >= 0) {
        mClick(document.getElementById('Loottery_3'), 'Charts3');
    }
    else if (location.href.indexOf("/TC22X5/") >= 0) {
        mClick(document.getElementById('Loottery_9'), 'Charts9');
    }
    else if (location.href.indexOf("/CQSSC/") >= 0) {
        mClick(document.getElementById('Loottery_28'), 'Charts28');
    }

    function setAColor(k) {
        var coll = document.all.tags("A");
        if (coll != null) {
            for (i = 0; i < coll.length; i++)
                coll[i].className = "";
        }
        k.className = "link_b";
    }

    function mClick(obj, id) {
        lastClickTabMenu = obj;
        var charts = document.getElementById("Charts").rows[0].cells;
        var l = charts.length;

        for (var i = 0; i < l; i++) {
            charts[i].style.display = "none";
        }
        document.getElementById(id).style.display = "";
    }

    //点击彩种
    function mChangeLottery(lid) {
        var dir = "";

        switch (lid) {
            case 5:
                dir = "SSQ/Default.aspx";
                break;
            case 39:
                dir = "CJDLT/Default.aspx";
                break;
            case 62:
                dir = "SYYDJ/Default.aspx";
                break;
            case 29:
                dir = "SHSSL/Default.aspx";
                break;
            case 6:
                dir = "FC3D/Default.aspx";
                break;
            case 13:
                dir = "QLC/Default.aspx";
                break;
            case 61:
                dir = "JXSSC/Default.aspx";
                break;
            case 63:
                dir = "PL3/Default.aspx";
                break;
            case 64:
                dir = "PL5/Default.aspx";
                break;
            case 3:
                dir = "7Xing/Default.aspx";
                break;
            case 9:
                dir = "TC22X5/Default.aspx";
                break;
            case 13:
                dir = "QLC/Default.aspx";
                break;
            case 28:
                dir = "CQSSC/Default.aspx";
                break;
        }

        var url = location.href;

        if (url.indexOf(dir) < 0) {
            url = url.substring(0, url.indexOf("TrendCharts") + 12) + dir;
            window.location.href = url;
        }
    }
  
</script>

