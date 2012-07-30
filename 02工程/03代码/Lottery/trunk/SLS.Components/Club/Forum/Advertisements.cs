using System;
using System.Data;
using System.Collections;
using System.Data.Common;
using System.Text;
using Discuz.Common;
using Discuz.Data;

using Discuz.Entity;
using Discuz.Config;

#if NET1
using System.Collections;
#else
using Discuz.Common.Generic;
#endif

namespace Discuz.Forum
{


    /// <summary>
    /// 广告类型
    /// </summary>
    public enum AdType
    {
        /// <summary>
        /// 头部横幅广告
        /// </summary>
        HeaderAd = 0x00,

        /// <summary>
        /// 尾部横幅广告
        /// </summary>
        FooterAd = 0x01,

        /// <summary>
        /// 页内文字广告
        /// </summary>
        PageWordAd = 0x02,

        /// <summary>
        /// 贴内广告
        /// </summary>
        InPostAd = 0x03,

        /// <summary>
        /// 浮动广告
        /// </summary>
        FloatAd = 0x04,

        /// <summary>
        /// 对联广告
        /// </summary>
        DoubleAd = 0x05,

        /// <summary>
        /// Silverlight媒体广告
        /// </summary>
        MediaAd = 0x06,

        /// <summary>
        /// 帖间通栏广告
        /// </summary>
        PostLeaderboardAd = 0x07,

        /// <summary>
        /// 分类间广告
        /// </summary>
        InForumAd = 0x08,

        /// <summary>
        /// 快速编辑器上方广告
        /// </summary>
        QuickEditorAd = 0x09,

        /// <summary>
        /// 快速编辑器背景广告
        /// </summary>
        QuickEditorBgAd = 0x0a

    }

    /// <summary>
    /// 广告操作类
    /// </summary>
    public class Advertisements
    {
        /// <summary>
        /// 按查询字符串得到广告列表
        /// </summary>
        /// <param name="selectstr">查询字符串</param>
        /// <returns>广告列表</returns>
        private static AdShowInfo[] GetAdsTable(string selectstr)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Advertisements") as DataTable;

            if (dt == null)
            {
                dt = DatabaseProvider.GetInstance().GetAdsTable();
                cache.AddObject("/Advertisements", dt);
            }

            DataRow[] drs = dt.Select(selectstr);
            int adlength = drs.Length;

            AdShowInfo[] adarray = new AdShowInfo[adlength];
           
            for (int i = 0; i < adlength; i++)
            {
                adarray[i] = new AdShowInfo();
                adarray[i].Advid = Convert.ToInt32(drs[i]["advid"].ToString());
                adarray[i].Displayorder = Convert.ToInt32(drs[i]["displayorder"].ToString());
                adarray[i].Code = drs[i]["code"].ToString().Trim();
                adarray[i].Parameters = drs[i]["parameters"].ToString().Trim();
            }
            
            return adarray;
        }


        /// <summary>
        /// 根据参数生成查询字符串
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="fid">版块id</param>
        /// <param name="adtype">广告类型</param>
        /// <returns>查询字符串</returns>
        public static string GetSelectStr(string pagename, int fid, AdType adtype)
        {
            string typestr = Convert.ToInt16(adtype).ToString();

            string selectstr = "";
            //if ((pagename != null) && (pagename != "") && (pagename == "indexad"))
            if(!string.IsNullOrEmpty(pagename) && (pagename=="indexad"))
            {
                selectstr = ",首页,";
            }
            else
            {
                if (fid > 0)
                {
                    selectstr += "," + fid + ",";
                }
            }

            if (selectstr == "")
            {
                selectstr = "type='" + typestr + "'  AND [targets] Like '%全部%'";
            }
            else
            {
                selectstr = "type='" + typestr + "'  AND ([targets] Like '%" + selectstr + "%' OR [targets] Like '%全部%')";
            }

            return selectstr;
        }


        /// <summary>
        /// 获得头部横幅广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetHeaderAdList(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.HeaderAd);
            return GetAdsTable(selectstr);
        }


        /// <summary>
        /// 返回头部随机一条横幅广告	
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static string GetOneHeaderAd(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.HeaderAd);
            string result = "";

            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 返回尾部横幅广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetFooterAdList(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.FooterAd);
            return GetAdsTable(selectstr);
        }

        /// <summary>
        /// 返回尾部随机一条横幅广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static string GetOneFooterAd(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.FooterAd);
            string result = "";

            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 返回页内文字广告列表
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetPageWordAdList(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.PageWordAd);
            return GetAdsTable(selectstr);
        }


        /// <summary>
        /// 返回页内文字广告html字符串
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static string[] GetPageWordAd(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.PageWordAd);

            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
#if NET1
            ArrayList list = new ArrayList();
#else
            List<string> list = new List<string>();
#endif
            if (adshowArray.Length < 1)
            {
                return new string[0];
            }
            foreach (AdShowInfo curadshow in adshowArray)
            {
                list.Add(curadshow.Code);
            }

#if NET1
            return (string[])list.ToArray(typeof(string))
#else
            return list.ToArray();
#endif
        }


        /// <summary>
        /// 返回贴内广告列表
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告内容</returns>
        public static AdShowInfo[] GetInPostAdList(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.InPostAd);
            return GetAdsTable(selectstr);
        }


        /// <summary>
        /// 获取帖内广告个数
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>广告个数</returns>
        public static int GetInPostAdCount(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.InPostAd);
            return GetAdsTable(selectstr).Length;

        }

        /// <summary>
        /// 返回贴内广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <param name="templatepath">模板路径</param>
        /// <param name="count">总数</param>
        /// <returns>贴内广告内容</returns>
        public static string GetInPostAd(string pagename, int forumid, string templatepath, int count)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.InPostAd);
            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            StringBuilder sb = new StringBuilder();
            if (adshowArray.Length > 0)
            {
                #region 原始代码

                sb.Append("<div style=\"display: none;\" id=\"ad_none\">\r\n");
                Random random = new Random();

                for (int i = 1; i <= count; i++)
                {//帖内下方的广告
#if NET1
                    AdShowInfoCollection tmp = new AdShowInfoCollection();
#else
                    List<AdShowInfo> tmp = new List<AdShowInfo>();
#endif
                    string[] parameter;
                    foreach (AdShowInfo adshow in adshowArray)
                    {
                        parameter = Utils.SplitString(adshow.Parameters.Trim(), "|", 9);
                        int position = Utils.StrToInt(parameter[7], -1);
                        if (position == 0)
                        {
                            string possibleflooridlist = parameter[8];
                            if (Utils.InArray(i.ToString(), possibleflooridlist, ",") || possibleflooridlist == "0")
                            {
                                tmp.Add(adshow);
                            }
                        }
                    }

                    if (tmp.Count > 0)
                    {
                        int number = random.Next(0, tmp.Count);
                        AdShowInfo ad = tmp[number];

                        sb.Append(string.Format("<div class=\"ad_textlink1\" id=\"ad_thread1_{0}_none\">{1}</div>\r\n", i, ad.Code));
                    }
                }

                for (int i = 1; i <= count; i++)
                {//帖内上方的广告
#if NET1
                    AdShowInfoCollection tmp = new AdShowInfoCollection();
#else
                    List<AdShowInfo> tmp = new List<AdShowInfo>();
#endif
                    string[] parameter;
                    foreach (AdShowInfo adshow in adshowArray)
                    {
                        parameter = Utils.SplitString(adshow.Parameters.Trim(), "|", 9);
                        int position = Utils.StrToInt(parameter[7], -1);
                        if (position == 1)
                        {
                            string possibleflooridlist = parameter[8];
                            if (Utils.InArray(i.ToString(), possibleflooridlist, ",") || possibleflooridlist == "0")
                            {
                                tmp.Add(adshow);
                            }
                        }
                    }

                    if (tmp.Count > 0)
                    {
                        int number = random.Next(0, tmp.Count);
                        AdShowInfo ad = tmp[number];

                        sb.Append(string.Format("<div class=\"ad_textlink2\" id=\"ad_thread2_{0}_none\">{1}</div>\r\n", i, ad.Code));
                    }
                }

                for (int i = 1; i <= count; i++)
                {//帖内右方的广告
#if NET1
					AdShowInfoCollection tmp = new AdShowInfoCollection();
#else
                    List<AdShowInfo> tmp = new List<AdShowInfo>();
#endif
                    foreach (AdShowInfo adshow in adshowArray)
                    {
                        string[] parameter = Utils.SplitString(adshow.Parameters.Trim(), "|", 9);
                        int position = Utils.StrToInt(parameter[7], -1);
                        if (position == 2)
                        {
                            string possibleflooridlist = parameter[8];
                            if (Utils.InArray(i.ToString(), possibleflooridlist, ",") || possibleflooridlist == "0")
                            {
                                tmp.Add(adshow);
                            }
                        }
                    }

                    if (tmp.Count > 0)
                    {
                        int number = random.Next(0, tmp.Count);
                        AdShowInfo ad = tmp[number];

                        sb.Append(string.Format("<div class=\"ad_pip\" id=\"ad_thread3_{0}_none\">{1}</div>\r\n", i, ad.Code));
                    }
                }

                sb.Append("</div>");
                sb.Append("<script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");
            }
            return sb.ToString();

                #endregion
        }

 


        /// <summary>
        /// 返回浮动广告列表
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>浮动广告内容</returns>
        public static AdShowInfo[] GetFloatAdList(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.FloatAd);
            return GetAdsTable(selectstr);
        }

        /// <summary>
        /// 返回浮动广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>浮动广告内容</returns>
        public static string GetFloatAd(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.FloatAd);

            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            string result = "";
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Parameters;
            }

            if (result == string.Empty)
            { 
                return ""; 
            }
            //初始化参数
            string[] parameter = result.Split('|');
            //
            string adhtml;
            if (parameter[0].ToLower() == "flash")
            {
                adhtml = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"" + parameter[2] + "\" height=\"" + parameter[3] + "\">"
                + "<param name=\"movie\" value=\"" + parameter[1] + "\" />"
                + "<param name=\"quality\" value=\"high\" />"
                + "<embed wmode=\"opaque\" width=\"" + parameter[2] + "\" height=\"" + parameter[3] + "\" src=\"" + parameter[1] + "\" type=\"application/x-shockwave-flash\"></embed>"
                + "</object>";
            }
            else
            {
                adhtml = "<img src=\"" + parameter[1] + "\" height=\"" + parameter[3] + "\" width=\"" + parameter[2] + "\" alt=\"" + parameter[1] + "\" border=\"0\">";

            }

            return "<script type='text/javascript'>theFloaters.addItem('floatAdv',10,'(document.body.clientHeight>document.documentElement.clientHeight ? document.documentElement.clientHeight :document.body.clientHeight)-" + parameter[3] + "-40','<a href=\"" + parameter[4] + "\" target=\"_blank\">" + adhtml + "</a>');</script>";
        }

        /// <summary>
        /// 返回对联广告列表
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>返回对联广告列表</returns>
        public static AdShowInfo[] GetDoubleAdList(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.DoubleAd);
            return GetAdsTable(selectstr);
        }


        /// <summary>
        /// 返回对联广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>返回广告内容</returns>
        public static string GetDoubleAd(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.DoubleAd);
            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            string result = "";
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Parameters;
            }
            if (result == "")
                return "" ;
            //初始化参数
            string[] parameter = result.Split('|');
            
            StringBuilder adhtml = new StringBuilder();

            //if (parameter[0].ToLower() == "flash")
            if (parameter[0].ToLower().CompareTo("flash")>0)
            {
                adhtml.AppendFormat("<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"" + parameter[2] + "\" height=\"" + parameter[3] + "\">");
                adhtml.AppendFormat("<param name=\"movie\" value=\"" + parameter[1] + "\" />");
                adhtml.AppendFormat("<param name=\"quality\" value=\"high\" />");
                adhtml.AppendFormat("<embed wmode=\"opaque\" width=\"" + parameter[2] + "\" height=\"" + parameter[3] + "\" src=\"" + parameter[1] + "\" type=\"application/x-shockwave-flash\"></embed>");
                adhtml.AppendFormat("</object>");
            }
            else
            {
                adhtml.AppendFormat("<img src=\"" + parameter[1] + "\" height=\"" + parameter[3] + "\" width=\"" + parameter[2] + "\" alt=\"" + parameter[1] + "\" border=\"0\">");

            }
            StringBuilder doublestr = new StringBuilder();

            doublestr.AppendFormat("<script type=\"text/javascript\">");
            doublestr.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv1','(document.body.clientWidth>document.documentElement.clientWidth ? document.documentElement.clientWidth :document.body.clientWidth )-" + parameter[2] + "-10',10,'<a href=\"" + parameter[4] + "\" target=\"_blank\">" + adhtml + "</a><br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\\'hand\\\'\" onClick=\"closeBanner();\">');");
            doublestr.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv2',10,10,'<a href=\"" + parameter[4] + "\" target=\"_blank\">" + adhtml.ToString() + "</a><br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\\'hand\\\'\" onClick=\"closeBanner();\">');");
            doublestr.AppendFormat("\n</script>");

            return doublestr.ToString();
        }


        /// <summary>
        /// 返回silverlight广告
        /// </summary>
        /// <param name="templatepath">模板路径</param>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>返回广告内容</returns>
        public static string GetMediaAd(string templatepath, string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.MediaAd);
            AdShowInfo[] adshowArray = GetAdsTable(selectstr);

            if (adshowArray.Length > 0)
            {
                return string.Format(adshowArray[0].Code, templatepath, pagename, forumid);
            }

            return "";
        }

        /// <summary>
        /// 获取视频广告的参数
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>获取视频广告的参数</returns>
        public static string[] GetMediaAdParams(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.MediaAd);
            AdShowInfo[] adshowArray = GetAdsTable(selectstr);

            if (adshowArray.Length > 0)
            {
                return adshowArray[0].Parameters.Split('|');
            }


            return new string[0];
        }

        /// <summary>
        /// 返回帖间通栏广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <returns>返回帖间通栏广告</returns>
        public static string GetOnePostLeaderboardAD(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.PostLeaderboardAd);
            string result = "";

            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 返回分类间广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <param name="count">总数</param>
        /// <returns>返回分类间广告</returns>
#if NET1
        public static string GetInForumAd(string pagename, int forumid, IndexPageForumInfoCollection topforum, string templatepath)
#else
        public static string GetInForumAd(string pagename, int forumid, List<IndexPageForumInfo> topforum, string templatepath)
#endif
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.InForumAd);
            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            StringBuilder result = new StringBuilder();
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result.Append("<div style=\"display: none\" id=\"ad_none\">\r\n");
                for (int i = 0; i < topforum.Count; i++)
                {
                    result.AppendFormat("<div class=\"ad_column\" id=\"ad_intercat_{0}_none\">{1}</div>\r\n", topforum[i].Fid, adshowArray[number].Code);
                }
                result.Append("</div>");
                result.Append("<script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");
            }

            return result.ToString();
        }

        /// <summary>
        /// 获取指定楼层的帖内广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">版块id</param>
        /// <param name="templatepath">模板路径</param>
        /// <param name="floor">获取指定楼层的帖内广告</param>
        public static string GetInPostAdXMLByFloor(string pagename, int forumid, string templatepath, int floor)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.InPostAd);
            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            StringBuilder sb = new StringBuilder();
            if (adshowArray.Length > 0)
            {
                Random random = new Random();
                //取帖内下方广告
#if NET1
                AdShowInfoCollection tmp = new AdShowInfoCollection();
#else
                List<AdShowInfo> tmp = new List<AdShowInfo>();
#endif
                foreach (AdShowInfo adshow in adshowArray)//可用的帖内下方广告
                {
                    string[] parameter = Utils.SplitString(adshow.Parameters.ToString().Trim(), "|", 9);
                    int position = Utils.StrToInt(parameter[7], -1);
                    if (position == 0)
                    {
                        string possibleflooridlist = parameter[8];
                        if (Utils.InArray(floor.ToString(), possibleflooridlist, ",") || possibleflooridlist == "0")
                        {
                            tmp.Add(adshow);
                        }
                    }
                }
                if (tmp.Count > 0)
                {
                    int number = random.Next(0, tmp.Count);
                    AdShowInfo ad = tmp[number];
                    sb.Append(string.Format("<ad_thread1><![CDATA[{0}]]></ad_thread1>", ad.Code));
                }

                tmp.Clear();//帖内上方
                foreach (AdShowInfo adshow in adshowArray)
                {
                    string[] parameter = Utils.SplitString(adshow.Parameters.ToString().Trim(), "|", 9);
                    int position = Utils.StrToInt(parameter[7], -1);
                    if (position == 1)
                    {
                        string possibleflooridlist = parameter[8];
                        if (Utils.InArray(floor.ToString(), possibleflooridlist, ",") || possibleflooridlist == "0")
                        {
                            tmp.Add(adshow);
                        }
                    }
                }
                if (tmp.Count > 0)
                {
                    int number = random.Next(0, tmp.Count);
                    AdShowInfo ad = tmp[number];
                    sb.Append(string.Format("<ad_thread2><![CDATA[{0}]]></ad_thread2>", ad.Code));
                }

                tmp.Clear();//帖内右侧
                foreach (AdShowInfo adshow in adshowArray)
                {
                    string[] parameter = Utils.SplitString(adshow.Parameters.ToString().Trim(), "|", 9);
                    int position = Utils.StrToInt(parameter[7], -1);
                    if (position == 2)
                    {
                        string possibleflooridlist = parameter[8];
                        if (Utils.InArray(floor.ToString(), possibleflooridlist, ",") || possibleflooridlist == "0")
                        {
                            tmp.Add(adshow);
                        }
                    }
                }
                if (tmp.Count > 0)
                {
                    int number = random.Next(0, tmp.Count);
                    AdShowInfo ad = tmp[number];
                    sb.Append(string.Format("<ad_thread3><![CDATA[{0}]]></ad_thread3>", ad.Code));
                }
            }
            return sb.ToString();
        }

  

        /// <summary>
        /// 返回快速发帖广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">板块id</param>
        /// <returns>返回快速发帖广告</returns>
        public static string GetQuickEditorAD(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.QuickEditorAd);
            string result = "";

            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }
            return result;
        }

        /// <summary>
        /// 快速编辑器背景广告
        /// </summary>
        /// <param name="pagename">页面名称</param>
        /// <param name="forumid">板块id</param>
        /// <returns>快速编辑器背景广告</returns>
        public static string[] GetQuickEditorBgAd(string pagename, int forumid)
        {
            string selectstr = GetSelectStr(pagename, forumid, AdType.QuickEditorBgAd);
            string result = "";
            AdShowInfo[] adshowArray = GetAdsTable(selectstr);
            if (adshowArray.Length > 0)
            {
                int number = new Random().Next(0, adshowArray.Length);
                result = adshowArray[number].Code;
            }

            return result.Split('\r');            
        }
    }
}
