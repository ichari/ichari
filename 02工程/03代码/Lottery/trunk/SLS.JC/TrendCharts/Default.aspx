<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="TrendCharts_Default" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=_Site.Name %>走势图-彩票走势图表和数据分析[竞彩足球，竞彩篮球，胜负彩，排列3/5，大乐透]－<%=_Site.Name %></title>
    <meta name="description" content="<%=_Site.Name %>为广大彩民提供竞彩足球，竞彩篮球，胜负彩，排列3/5，大乐透等彩票开奖号码预测分析。" />
    <meta name="keywords" content="彩票走势图,彩票走势图表,数据分析" />
    <link href="../Style/dahecp.css" rel="stylesheet" type="text/css" />
    <link href="css.css" rel="stylesheet" type="text/css" />
    <link href="../Style/public.css" rel="stylesheet" type="text/css" />
    <link href="../Style/part_b.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="../favicon.ico" />
    <script type="text/javascript" src="/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#WebHead1_currentMenu").val("mChart");
        });
    </script>
</head>
<body class="gybg">
<form id="form1" runat="server">
<asp:PlaceHolder ID="phHead" runat="server"></asp:PlaceHolder>
<div class="w980 png_bg2"> 
  <!-- 内容开始 -->
  <div class="w970">
    <div class="bsc5">
    	<div class="lists">
        	<div class="title bg1x bgp5"><span class="bg1x bgp1">双色球数据分析</span></div>
            <div class="list">
            	<div class="fl"><img src="/Images/_New/ben_09.png" width="105" height="122" /></div>
            	<ul class="fr">
                	<li><a href="SSQ/SSQ_CGXMB.aspx" target="mainFrame">常规项目表走势图</a></li> 
					<li><a href="SSQ/SSQ_ZHFB.aspx" target="mainFrame">综合分布走势图</a></li>					
					<li><a href="SSQ/SSQ_3FQ.aspx" target="mainFrame">3分区分布走势图</a></li>					
                    <li><a href="SSQ/SSQ_DX.aspx" target="mainFrame">大小分析走势图</a></li>
					<li><a href="SSQ/SSQ_JO.aspx" target="mainFrame">奇偶分析走势图</a></li>				
					<li><a href="SSQ/SSQ_ZH.aspx" target="mainFrame">质合分析走势图</a></li>
                    <li><a href="SSQ/SSQ_HL.aspx" target="mainFrame">红蓝球走势图</a></li>
					<li><a href="SSQ/SSQ_BQZST.aspx" target="mainFrame">蓝球综合走势图</a></li>                  
                </ul>
            </div>
        </div>
        <div class="lists">
        	<div class="title bg1x bgp5"><span class="bg1x bgp1">福彩3D</span></div>
            <div class="list">
            	<div class="fl"><img src="/Images/_New/ben_10.png" width="105" height="122" /></div>
            	<ul class="fr">
                	<li><a href="FC3D/FC3D_ZHXT.aspx" target="mainFrame">综合分布遗漏走势图</a></li>
					<li><a href="FC3D/FC3D_C3YS.aspx" target="mainFrame">除三余数形态遗漏走势图</a></li>
					<li><a href="FC3D/FC3D_ZHZST.aspx" target="mainFrame">质合形态遗漏走势图</a></li>
					<li><a href="FC3D/FC3D_XTZST.aspx" target="mainFrame">形态走势遗漏走势图</a></li>
					<li><a href="FC3D/FC3D_KDZST.aspx" target="mainFrame">跨度走势遗漏走势图</a></li>
					<li><a href="FC3D/FC3D_HZZST.aspx" target="mainFrame">和值走势遗漏走势图</a></li>			
					<li><a href="FC3D/FC3D_DZXZST.aspx" target="mainFrame">大中小形态遗漏走势图</a></li>
					<li><a href="FC3D/FC3D_C3YS_HTML.aspx" target="mainFrame">除三余数号码分区表走势图</a></li>
					<li><a href="FC3D/FC3D_2CHW.aspx" target="mainFrame">二次和尾差尾查询表走势图</a></li>
					<li><a href="FC3D/FC3D_DSHM.aspx" target="mainFrame">单双点号码分区表走势图</a></li>
					<li><a href="FC3D/FC3D_DTHM.aspx" target="mainFrame">胆托号码分区表走势图</a></li>
					<li><a href="FC3D/FC3D_DX_JO.aspx" target="mainFrame">大小—奇偶号码分区表走势图</a></li>
					<li><a href="FC3D/FC3D_HSWH.aspx" target="mainFrame">和数尾号码分区表走势图</a></li>
					<li><a href="FC3D/FC3D_HSZ.aspx" target="mainFrame">和数值号码分区表走势图</a></li>
					<li><a href="FC3D/FC3D_KDZH.aspx" target="mainFrame">跨度组合分区表走势图</a></li>
					<li><a href="FC3D/FC3D_JO_DX.aspx" target="mainFrame">奇偶—大小号码分区表走势图</a></li>
					<li><a href="FC3D/FC3D_LHZH.aspx" target="mainFrame">连号组合分区表走势图</a></li>
					<li><a href="FC3D/FC3D_ZS.aspx" target="mainFrame">质数号码分区表走势图</a></li>                    
                </ul>
            </div>
        </div>
        <div class="lists">
        	<div class="title bg1x bgp5"><span class="bg1x bgp1">七乐彩</span></div>
            <div class="list">
            	<div class="fl"><img src="/Images/_New/ben_11.png" width="116" height="120" /></div>
            	<ul class="fr">
                	<li><a href="QLC/7LC_CGXMB.aspx" target="mainFrame">七乐彩常规项目表走势图</a></li>
                    <li><a href="QLC/7LC_ZHFB.aspx" target="mainFrame">七乐彩综合分布走势图</a></li>
                </ul>
            </div>
        </div>
        <div class="lists">
        	<div class="title bg1x bgp5"><span class="bg1x bgp1">时时彩</span></div>
            <div class="list">
            	<div class="fl"><img src="/Images/_New/ben_12.png" width="130" height="120" /></div>
            	<ul class="fr">
                    <li><a href="CQSSC/SSC_5X_ZST.aspx" target="mainFrame">五星基本走势图</a></li>
                	<%--
                    <li><a href="JXSSC/SSC_5X_ZHFB.aspx" target="mainFrame">标准五星综合走势图</a></li>
					<li><a href="JXSSC/SSC_5X_ZST.aspx" target="mainFrame">标准五星走势图</a></li>
					<li><a href="JXSSC/SSC_5X_HZZST.aspx" target="mainFrame">五星和值走势图</a></li>
					<li><a href="JXSSC/SSC_5X_KDZST.aspx" target="mainFrame">五星跨度走势图</a></li>
					<li><a href="JXSSC/SSC_5X_PJZZST.aspx" target="mainFrame">五星平均值走势图</a></li>
					<li><a href="JXSSC/SSC_5X_DXZST.aspx" target="mainFrame">五星大小走势图</a></li>
					<li><a href="JXSSC/SSC_5X_JOZST.aspx" target="mainFrame">五星奇偶走势图</a></li>
					<li><a href="JXSSC/SSC_5X_ZHZST.aspx" target="mainFrame" "'"="">五星质合走势图</a></li>
					<li><a href="JXSSC/SSC_4X_ZHFB.aspx" target="mainFrame">标准四星综合走势图</a></li>
					<li><a href="JXSSC/SSC_4X_ZST.aspx" target="mainFrame">标准四星走势图</a></li>
					<li><a href="JXSSC/SSC_4X_HZZST.aspx" target="mainFrame">四星和值走势图</a></li>
					<li><a href="JXSSC/SSC_4X_KDZST.aspx" target="mainFrame">四星跨度走势图</a></li>
					<li><a href="JXSSC/SSC_4X_PJZZST.aspx" target="mainFrame">四星平均值走势图</a></li>
					<li><a href="JXSSC/SSC_4X_DXZST.aspx" target="mainFrame">四星大小走势图</a></li>
					<li><a href="JXSSC/SSC_4X_JOZST.aspx" target="mainFrame">四星奇偶走势图</a></li>
					<li><a href="JXSSC/SSC_4X_ZHZST.aspx" target="mainFrame">四星质合走势图</a></li>
					<li><a href="JXSSC/SSC_3X_ZHZST.aspx" target="mainFrame">标准三星综合</a></li>
					<li><a href="JXSSC/SSC_3X_ZST.aspx" target="mainFrame">标准三星走势图</a></li>
					<li><a href="JXSSC/SSC_3X_HZZST.aspx" target="mainFrame">三星和值走势图</a></li>
					<li><a href="JXSSC/SSC_3X_HZWZST.aspx" target="mainFrame">三星和值尾走势图</a></li>
					<li><a href="JXSSC/SSC_3X_KDZST.aspx" target="mainFrame">三星跨度走势图</a></li>
					<li><a href="JXSSC/SSC_3X_DXZST.aspx" target="mainFrame">三星大小走势图</a></li>
					<li><a href="JXSSC/SSC_3X_JOZST.aspx" target="mainFrame">三星奇偶走势图</a></li>
					<li><a href="JXSSC/SSC_3X_ZhiHeZST.aspx" target="mainFrame">三星质合走势图</a></li>
					<li><a href="JXSSC/SSC_3X_DX_012_ZST.aspx" target="mainFrame">单选012路走势图</a></li>
					<li><a href="JXSSC/SSC_3X_ZX_012_ZST.aspx" target="mainFrame">组选012路走势图</a></li>
					<li><a href="JXSSC/SSC_2X_ZHFBZST.aspx" target="mainFrame">标准二星综合走势图</a></li>
					<li><a href="JXSSC/SSC_2X_HZZST.aspx" target="mainFrame">二星和值走势图</a></li>
					<li><a href="JXSSC/SSC_2X_HZWZST.aspx" target="mainFrame">二星和尾走势图</a></li>
					<li><a href="JXSSC/SSC_2XPJZZST.aspx" target="mainFrame">二星均值走势图</a></li>
					<li><a href="JXSSC/SSC_2X_KDZST.aspx" target="mainFrame">二星跨度走势图</a></li>
					<li><a href="JXSSC/SSC_2X_012ZST.aspx" target="mainFrame">二星012路走势图</a></li>
					<li><a href="JXSSC/SSC_2X_MaxZST.aspx" target="mainFrame">二星最大值走势图</a></li>
					<li><a href="JXSSC/SSC_2X_MinZST.aspx" target="mainFrame">二星最小值走势图</a></li>
					<li><a href="JXSSC/SSC_2X_DXDSZST.aspx" target="mainFrame">大小单双走势图</a></li>  
                    --%>               
                </ul>
            </div>
        </div>
    </div>
  </div>
</div>
<!-- 内容结束 -->
<div class=" w980 png_bg1"></div>
<asp:PlaceHolder ID="phFoot" runat="server"></asp:PlaceHolder>
</form>
</body>
</html>
