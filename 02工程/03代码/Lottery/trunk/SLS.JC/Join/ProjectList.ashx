<%@ WebHandler Language="C#" Class="ProjectList" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

public class ProjectList : IHttpHandler
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

        string order = "";
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
        string PlayTypeID = "";
        string Name = "";
        string State = "";
        string lotteryID = "74";

        if (!string.IsNullOrEmpty(context.Request["State"]))
        {
            State = Shove._Web.Utility.GetRequest(context, "State");

            switch (State)
            {
                case "-1":
                    Condition += "";
                    break;
                case "1":
                    Condition += "Schedule < 100 and QuashStatus = 0";
                    break;
                case "2":
                    Condition += "QuashStatus <> 0";
                    break;
                case "100":
                    Condition += "Schedule >= 100";
                    break;
                default:
                    break;
            }
        }
        else
        {
            Condition += "Schedule < 100 and QuashStatus = 0";
        }

        if (!string.IsNullOrEmpty(context.Request["PlayTypeID"]))
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " and ";
            }
            
            PlayTypeID = Shove._Web.Utility.GetRequest(context, "PlayTypeID");
            Condition += "PlayTypeID=" + PlayTypeID;
        }

        if (!string.IsNullOrEmpty(context.Request["Name"]))
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " and ";
            }
            
            Name = Shove._Web.Utility.GetRequest(context, "Name");
            Condition += "InitiateName like '%" + Name + "%'";
        }

        if (!string.IsNullOrEmpty(context.Request["lotteryID"]))
        {
            lotteryID = Shove._Web.Utility.GetRequest(context, "lotteryID");
        }

        string Key = "join_ProjectList_lotteryID" + lotteryID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select a.ID,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName, a.Multiple, Share, BuyedShare, Schedule, AssureMoney, ")
                   .AppendLine("	InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                   .AppendLine("from")
                   .AppendLine("	(")
                   .AppendLine("		select top 1 ID, EndTime,IsOpened from T_Isuses where getdate() between StartTime and EndTime and LotteryID =" + lotteryID)
                   .AppendLine("	) as d")
                   .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID  and a.isOpened = 0 and a.Share > 1")
                   .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                   .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                   .AppendLine("order by a.QuashStatus asc,IsFull asc, a.AtTopStatus desc, a.Schedule desc");

            dt = MSSQL.Select(sql.ToString());

            if (dt == null)
            {
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(Key, dt, 0);
            }
        }

        if (string.IsNullOrEmpty(order))
        {
            order = "QuashStatus asc, IsFull asc, AtTopStatus desc, Schedule desc";
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

        float Money = 0;
        int Share = 0;
        double Schedule = 0;
        double BuyedShare = 0;
        string Surplus = "";

        for (int i = (pageindex - 1) * perPageRowCount; i < Count; i++)
        {
            Money = Shove._Convert.StrToFloat(dr[i]["Money"].ToString(), 0);
            Share = Shove._Convert.StrToInt(dr[i]["Share"].ToString(), 0);
            Schedule = Shove._Convert.StrToDouble(dr[i]["Schedule"].ToString(), 0);
            BuyedShare = Shove._Convert.StrToInt(dt.Rows[i]["BuyedShare"].ToString(), 0);

            Surplus = (Share - BuyedShare).ToString();

            Surplus = (Share * (1 - Schedule * 0.01)).ToString();

            if (Surplus.IndexOf(".") >= 0)
            {
                Surplus = (Shove._Convert.StrToInt(Surplus.Substring(0, Surplus.IndexOf(".")), 0) + 1).ToString();
            }
                
            sbContent.Append("<tr><td><span>" + (i + 1).ToString() + "</span></td>");
            sbContent.Append("<td>" + dr[i]["InitiateName"].ToString() + "</td>");
            sbContent.Append("<td><div style=\"background-image:url(Images/gold.gif);height:38px; background-position:left center; width:" + (9 * Shove._Convert.StrToInt(dr[i]["Level"].ToString(), 0)).ToString() + "px;background-repeat:repeat-x;\">&nbsp;</div></td>");
            sbContent.Append("<td>" + dr[i]["PlayTypeName"].ToString() + "</td>");
            sbContent.Append("<td><span>" + dr[i]["Multiple"].ToString() + "</span> 倍</td>");
            sbContent.Append("<td><span>" + Money.ToString() + "</span> 元</td>");
            sbContent.Append("<td><span>" + (Money / Share).ToString() + "</span> 元</td>");
            if (dr[i]["Schedule"].ToString() == "100")
            {
                sbContent.Append("<td><span>满员</span></td>");
            }
            else if (dr[i]["QuashStatus"].ToString() != "0")
            {
                sbContent.Append("<td><span>已撤单</span></td>");
            }
            else
            {
                if (Shove._Convert.StrToDouble(dr[i]["AssureMoney"].ToString(), 0) > 0)
                {
                    sbContent.Append("<td><span>" + dr[i]["Schedule"].ToString() + "%+<span class=red>" + (Shove._Convert.StrToDouble(dr[i]["AssureMoney"].ToString(), 0) / Money * 100).ToString("N") + "%(保)</span></span></td>");
                }
                else
                {
                    sbContent.Append("<td><span>" + dr[i]["Schedule"].ToString() + "%</span></td>");
                }
            }
            sbContent.Append("<td><span>" + Surplus + "</span> 份");

            if (dr[i]["QuashStatus"].ToString() != "0" || dr[i]["Schedule"].ToString() == "100")
            {
                sbContent.Append("<td>--</td>");
                sbContent.Append("<td><a href=../Home/Room/Scheme.aspx?id=" + dr[i]["ID"].ToString() + " target=_blan title=点击查看方案详细信息>详情</a></td></tr>");
            }
            else
            {
                sbContent.Append("<td><input type=\"text\" value=\"1\" size=\"4\" class=\"Share\" /></td>");
                sbContent.Append("<td><a href=\"#\" class=\"join\"><img src=\"images/btn_cy.gif\" alt=\"参与\" align=\"middle\" mid=\"" + dr[i]["ID"].ToString() + "\" /></a>&nbsp;<a href=../Home/Room/Scheme.aspx?id=" + dr[i]["ID"].ToString() + " target=_blan title=点击查看方案详细信息>详情</a></td></tr>");
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