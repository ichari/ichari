<%@ WebHandler Language="C#" Class="GuoGuan" %>

using System;
using System.Web;
using System.Data;
using System.Globalization;
using System.Text;

using Shove.Database;

public class GuoGuan : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";

        int pageindex;
        int.TryParse(context.Request["p"], out pageindex);

        string order = "WinMoney desc";
        if (!string.IsNullOrEmpty(context.Request["orderby"]))
        {
            string[] strarr = context.Request["orderby"].ToString().Split('_');
            if (strarr[1] == "0")
                order = strarr[0] + " asc";
            else
                order = strarr[0] + " desc";
            
        }
        
        if (pageindex == 0)
            pageindex = 1;

        int PageNum = 20;
        if (!string.IsNullOrEmpty(context.Request["EachPageNum"]))
        {
            PageNum = Shove._Convert.StrToInt(context.Request["EachPageNum"].ToString(), 20);
        }

        string Condition = "";
        string PlayTypeID = "7201";
        string Name = "";

        if (!string.IsNullOrEmpty(context.Request["PlayTypeID"]))
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " and ";
            }
            
            PlayTypeID = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "PlayTypeID"));
        }

        Condition += "PlayTypeID=" + PlayTypeID;

        if (!string.IsNullOrEmpty(context.Request["Name"]))
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " and ";
            }
            
            Name = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "Name"));
            Condition += "(Name like '%" + Name + "%' or SchemeNumber='" + Name + "')";
        }

        if (!string.IsNullOrEmpty(context.Request["d"]))
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " and ";
            }

            String sTime = String.Empty, eTime = String.Empty;

            DateTime dtDateTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.FilteSqlInfusion(context.Request["d"]), DateTime.Now.ToString());
            
            try
            {
                sTime = dtDateTime.ToString("yyyy-MM-dd");
                eTime = dtDateTime.AddDays(1).ToString("yyyy-MM-dd");
            }
            catch (FormatException ex)
            {
                return;
            }

            Condition += string.Format("DateTime>=#{0}# and DateTime<=#{1}#", sTime, eTime);
        }

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_GuoGuan_SFC");

        if (dt == null)
        {
            // 方案表
            DataTable dts = MSSQL.Select("select T_BuyWays.*, ts.DateTime as DateTime, ts.WinMoney from T_BuyWays inner join T_Schemes ts on T_BuyWays.SchemeID = ts.ID and Type = 1 and ts.DateTime between CONVERT(VARCHAR(10), DATEADD( DAY, -16,GETDATE()) ,120) and CONVERT(VARCHAR(10), DATEADD( DAY, 0,GETDATE()) ,120) order by ts.WinMoney");
            // 擂台方案表
            //DataTable dtcs = MSSQL.Select("select T_BuyWays.*, cs.DateTime as DateTime, cs.WinMoney from T_BuyWays inner join T_ChallengeScheme cs on T_BuyWays.SchemeID = cs.ID and Type = 2 and cs.DateTime between CONVERT(VARCHAR(10), DATEADD( DAY, -16,GETDATE()) ,120) and CONVERT(VARCHAR(10), DATEADD( DAY, 0,GETDATE()) ,120) order by cs.WinMoney");   
            //// 擂台保存方案表
            //DataTable dtcss = MSSQL.Select("select T_BuyWays.*, css.DateTime as DateTime, css.WinMoney from T_BuyWays inner join T_ChallengeSaveScheme css on T_BuyWays.SchemeID = css.ID and Type = 3 and css.DateTime between CONVERT(VARCHAR(10), DATEADD( DAY, -16,GETDATE()) ,120) and CONVERT(VARCHAR(10), DATEADD( DAY, 0,GETDATE()) ,120) order by css.WinMoney");

            dt = dts.Clone();

            // 装载方案表数据
            foreach (DataRow drs in dts.Rows)
            {
                dt.ImportRow(drs);
            }
            //// 合并擂台方案表
            //foreach (DataRow drs in dtcs.Rows)
            //{
            //    dt.ImportRow(drs);
            //}
            //// 合并擂台保存方案表
            //foreach (DataRow drs in dtcss.Rows)
            //{
            //    dt.ImportRow(drs);
            //}            
            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(128)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_GuoGuan_SFC", dt, 1200);
            }
        }
        
        DataRow[] dr = dt.Select(Condition, order);
        
        int perPageRowCount = PageNum;
        int rowCount = dr.Length;
        int pageCount = rowCount % perPageRowCount == 0 ? rowCount / perPageRowCount : rowCount / perPageRowCount + 1;

        if (pageindex > pageCount)
        {
            pageindex = pageCount;
        }

        if (pageindex < 1)
        {
            pageindex = 1;
        }

        int Count = 0;

        if (perPageRowCount * pageindex > rowCount)
        {
            Count = rowCount;
        }
        else
        {
            Count = perPageRowCount * pageindex;
        }

        StringBuilder sbContent = new StringBuilder();

        for (int i = (pageindex - 1) * perPageRowCount; i < Count; i++)
        {
            //if (dr[i]["Type"].ToString().Equals("1"))
            //{
            //    sbContent.Append("<tr class=\"ni2\"><td></td>");
            //}
            //else
            //{
            //    sbContent.Append("<tr class=\"ni2\"><td>" + dr[i]["SchemeNumber"].ToString() + "</td>");
            //}
            sbContent.Append("<tr class=\"ni2\">");
            sbContent.Append("<td><a href=\"../Home/Web/Score.aspx?id=" + dr[i]["UserID"].ToString() + "&LotteryID=72\" class=\"dl\" target=\"_blank\">" + dr[i]["Name"].ToString() + "</a></td>");
            sbContent.Append("<td>" + dr[i]["Count1"].ToString() + "</td>");
            sbContent.Append("<td>" + dr[i]["GameNumber"].ToString() + "</td>");
            sbContent.Append("<td style=\"line-height:16px;\">" + dr[i]["BuyWays"].ToString() + "</td>");
            sbContent.Append("<td class=\"fb c_org\">" + dr[i]["Count2"].ToString() + "</td>");
            sbContent.Append("<td>" + dr[i]["GameNumber2"].ToString() + "</td>");
            sbContent.Append("<td>" + (Shove._Convert.StrToDouble(dr[i]["GameNumber2"].ToString(), 0) / Shove._Convert.StrToDouble(dr[i]["GameNumber"].ToString(), 0) * 100.00).ToString("F2") + "%</td>");
            sbContent.Append("<td class=\"red\">" + Shove._Convert.StrToDouble(dr[i]["WinMoney"].ToString(), 0).ToString("F2") + "</td>");

            string Type = "";
            if (dr[i]["Type"].ToString().Equals("1"))
            {
                Type = "竞彩方案";
                sbContent.Append("<td>" + Type + "</td>");
                sbContent.Append("<td><a class=\"dl\" href=\"../Home/Room/Scheme.aspx?id=" + dr[i]["SchemeID"].ToString() + "\" target=\"_blank\">查看</a></td></tr>");
            }
            else if (dr[i]["Type"].ToString().Equals("2"))
            {
                Type = "[擂台方案]";
                sbContent.Append("<td>" + Type + "</td>");
                sbContent.Append("<td><a class=\"dl\" href=\"javascript:showWinOpen(|/Challenge/ChallengeSchemes.aspx?SchemesID=" + dr[i]["SchemeID"].ToString() + "|,|方案详情|,230,600)\">查看</td></tr>");
            }
        }

        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Content", typeof(System.String));

        DataRow drNew = dtNew.NewRow();
        drNew["Content"] = sbContent.ToString();
        dtNew.Rows.Add(drNew);

        dtNew.AcceptChanges();

        DataSet ds = new DataSet();

        ds.Tables.Add(dtNew);

        DataTable dtPage = new DataTable();

        dtPage.Columns.Add("page", typeof(System.String));
        
        StringBuilder sb = new StringBuilder();

        sb.Append("<span class=\"jilu\">共" + pageCount.ToString() + "页，" + dr.Length + "条记录</span><span id=\"first\"><a href=\"#\" onclick = \"InitData(0);\">首页</a></span>");

        if (pageindex == 1)
        {
            sb.Append("<span class=\"disabled\">« 上一页</span>");
        }
        else
        {
            sb.Append("<span><a href=\"#\" onclick = \"InitData(" + (pageindex - 2).ToString() + ");\">« 上一页</a></span>");
        }

        for (int i = 0; i < pageCount; i++)
        {
            if (i == pageindex - 1)
            {
                sb.Append("<span class=\"current\" onclick = \"InitData(" + i.ToString() + ");\">" + (i + 1).ToString() + "</span>");

                continue;
            }

            if ((i < pageindex + 4 || i < 9) && (i > pageindex - 6 || i > pageCount - 10))
            {
                sb.Append("<a href=\"#\" onclick = \"InitData(" + i.ToString() + ");\">" + (i + 1).ToString() + "</a>");
            }
        }

        if (pageindex == pageCount)
        {
            sb.Append("<span class=\"disabled\">下一页 »</span>");
        }
        else
        {
            sb.Append("<span><a href=\"#\" onclick = \"InitData(" + (pageindex).ToString() + ");\">下一页 »</a></span>");
        }

        sb.Append("<span id=\"last\" value=\"" + pageCount.ToString() + "\"><a href=\"#\" onclick = \"InitData(" + (pageCount - 1).ToString() + ");\">尾页</a></span>");

        DataRow drPage = dtPage.NewRow();

        drPage["page"] = sb.ToString();

        dtPage.Rows.Add(drPage);
        dtPage.AcceptChanges();
        ds.Tables.Add(dtPage);
        
        string jsonData = JsonHelper.GetJsonByDataset(ds);
            
        context.Response.Write(jsonData);  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}