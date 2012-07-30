<%@ WebHandler Language="C#" Class="SchemeList" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

public class SchemeList : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";

        string lotteryID = "5";

        if (!string.IsNullOrEmpty(context.Request["lotteryID"]))
        {
            lotteryID = Shove._Web.Utility.GetRequest(context, "lotteryID");
        }

        int TopNum = 10;

        if (!string.IsNullOrEmpty(context.Request["TopNum"]))
        {
            TopNum = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest(context, "TopNum"), 10);
        }

        string Key = "join_SchemeList_lotteryID" + lotteryID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select top " + TopNum.ToString() + " a.ID,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName, a.Multiple, Share, BuyedShare, Schedule, AssureMoney, ")
                   .AppendLine("	InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                   .AppendLine("from")
                   .AppendLine("	(")
                   .AppendLine("		select top 1 ID, EndTime,IsOpened from T_Isuses where getdate() between StartTime and EndTime and LotteryID =" + lotteryID)
                   .AppendLine("	) as d")
                   .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID  and a.isOpened = 0 and a.Share > 1 and a.buyed = 0 and a.QuashStatus = 0 and a.Schedule < 100")
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

        StringBuilder sbContent = new StringBuilder();

        float Money = 0;
        int Share = 0;
        double Schedule = 0;
        double BuyedShare = 0;
        string Surplus = "";

        for (int i = 0; i < TopNum; i++)
        {
            if (i >= dt.Rows.Count)
            {
                sbContent.Append("<tr><td><span>&nbsp;</span></td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");

                continue;
            }
            
            Money = Shove._Convert.StrToFloat(dt.Rows[i]["Money"].ToString(), 0);
            Share = Shove._Convert.StrToInt(dt.Rows[i]["Share"].ToString(), 0);
            Schedule = Shove._Convert.StrToDouble(dt.Rows[i]["Schedule"].ToString(), 0);
            BuyedShare = Shove._Convert.StrToInt(dt.Rows[i]["BuyedShare"].ToString(), 0);

            Surplus = (Share - BuyedShare).ToString();

            if (Surplus.IndexOf(".") >= 0)
            {
                Surplus = (Shove._Convert.StrToInt(Surplus.Substring(0, Surplus.IndexOf(".")), 0) + 1).ToString();
            }
                
            sbContent.Append("<tr><td><span>" + (i + 1).ToString() + "</span></td>");
            sbContent.Append("<td>" + dt.Rows[i]["InitiateName"].ToString() + "</td>");
            sbContent.Append("<td><div style=\"background-image:url(/Join/Images/gold.gif);height:38px; background-position:left center; width:" + (9 * Shove._Convert.StrToInt(dt.Rows[i]["Level"].ToString(), 0)).ToString() + "px;background-repeat:repeat-x;\">&nbsp;</div></td>");
            sbContent.Append("<td><span>" + dt.Rows[i]["Multiple"].ToString() + "</span> 倍</td>");
            sbContent.Append("<td><span>" + Money.ToString() + "</span> 元</td>");
            sbContent.Append("<td><span>" + (Money / Share).ToString() + "</span> 元</td>");
            if (dt.Rows[i]["Schedule"].ToString() == "100")
            {
                sbContent.Append("<td><span>满员</span></td>");
            }
            else if (dt.Rows[i]["QuashStatus"].ToString() != "0")
            {
                sbContent.Append("<td><span>已撤单</span></td>");
            }
            else
            {
                if (Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0) > 0)
                {
                    sbContent.Append("<td><span>" + dt.Rows[i]["Schedule"].ToString() + "%+<span class=red>" + (Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0) / Money * 100).ToString("N") + "%(保)</span></span></td>");
                }
                else
                {
                    sbContent.Append("<td><span>" + dt.Rows[i]["Schedule"].ToString() + "%</span></td>");
                }
            }
            sbContent.Append("<td><span>" + Surplus + "</span> 份");

            if (dt.Rows[i]["QuashStatus"].ToString() != "0" || dt.Rows[i]["Schedule"].ToString() == "100")
            {
                sbContent.Append("<td>--</td>");
                sbContent.Append("<td><a href=\"/Home/Room/Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "\" target=_blan title=点击查看方案详细信息>详情</a></td></tr>");
            }
            else
            {
                sbContent.Append("<td><input type=\"text\" value=\"1\" size=\"4\" class=\"Share\" /></td>");
                sbContent.Append("<td><a href=\"#\" class=\"join\"><img src=\"/Join/images/btn_cy.gif\" alt=\"参与\" align=\"middle\" mid=\"" + dt.Rows[i]["ID"].ToString() + "\" /></a>&nbsp;<a href=\"/Home/Room/Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "\" target=_blan title=点击查看方案详细信息>详情</a></td></tr>");
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
        
        string jsonData = JsonHelper.GetJsonByDataset(ds);
            
        context.Response.Write(jsonData);  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}