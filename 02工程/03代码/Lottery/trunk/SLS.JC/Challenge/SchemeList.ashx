<%@ WebHandler Language="C#" Class="SchemeList" %>

using System;
using System.Web;
using System.Data;
using System.Text;

public class SchemeList : IHttpHandler {
    
    private string userName = "";
    private string datetime = "";
    
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

        userName = context.Request["tbUserName"];
        datetime = context.Request["tbYser"];
        
        string order = "";
        if (!string.IsNullOrEmpty(context.Request["orderby"]))
        {
            string[] strarr = context.Request["orderby"].ToString().Split('_');
            if (strarr[1] == "0")
                order = strarr[0] + " asc";
            else
                order = strarr[0] + " desc";

        }

        //
        if (pageindex == 0)
            pageindex = 1;

        int PageNum = 20;
        if (!string.IsNullOrEmpty(context.Request["EachPageNum"]))
        {
            PageNum = Shove._Convert.StrToInt(context.Request["EachPageNum"].ToString(), 20);
        }

        string Condition = "";

        DataTable dts = Shove._Web.Cache.GetCacheAsDataTable("DataCache_ChallengeList_72_BindUserNewBetSchemesContent");

        if (dts == null)
        {
            dts = Shove.Database.MSSQL.Select("select top 60 row_number() over (order by ([DateTime]) desc) as Ranking,s.ID ,DateTime,u.Name ,s.LotteryNumber,s.Odds from T_ChallengeScheme as s , T_Users as u where u.ID = s.InitiateUserId order by Ranking");

            if (dts == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(133)", this.GetType().BaseType.FullName);

                return;
            }

            if (dts.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_ChallengeList_72_BindUserNewBetSchemesContent", dts, 100);
            }
        }



        DataTable dt = dts;
        // 处理Dt
        dt = GetUserNewBetContent(dt);

        DataRow[] dr = dt.Select(Condition, "Ranking");

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
            sbContent.Append("<tr class=\"sk1\" align=\"center\"><td width=\"5%\">" + dr[i]["Ranking"].ToString() + "</td>");
            sbContent.Append("<td width=\"16%\">" + dr[i]["Name"].ToString() + "</td>");
            sbContent.Append("<td width=\"13%\">" + dr[i]["BetCount"].ToString() + "</td>");
            sbContent.Append("<td>" + dr[i]["LotteryNumber"].ToString() + "</td>");
            sbContent.Append("<td width=\"13%\">" + dr[i]["BetWay"].ToString() + "</td>");
            sbContent.Append("<td width=\"10%\"><a href=\"javascript:showWinOpen('ChallengeSchemes.aspx?SchemesID=" + dr[i]["ID"] + "','方案详情',190,600)\">查看</a></td>");
            sbContent.Append("</tr>");
        }
        
        if (Count < 1)
        {
            sbContent.AppendLine("<tr><td colspan=\"5\">"+datetime+"没有您要找的数据</td></tr>");
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

        string jsonData = "{\"Content\":\"" + sbContent.ToString().Replace("\"", "\\\"").Replace("'", "\'") + "\",\"page\":\"" + sb.ToString().Replace("\"", "\\\"").Replace("'", "\'") + "\"}";
                      
        context.Response.Write(jsonData);  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    /// <summary>
    /// 处理 方案
    /// </summary>
    /// <param name="dtNewUserMessage"></param>
    private DataTable GetUserNewBetContent(DataTable dtNewUserMessage)
    {

        try
        {
            dtNewUserMessage.Columns.Add("BetWay", Type.GetType("System.String"));
            dtNewUserMessage.Columns.Add("BetCount", Type.GetType("System.String"));
        }
        catch { }

        foreach (DataRow dr in dtNewUserMessage.Rows)
        {
            string LotteryNumber = "";

            int BetCount = 0;// 投注场次
            string Odds = "";// 投注赔率
            try
            {
                LotteryNumber = dr["LotteryNumber"].ToString();

                Odds = dr["Odds"].ToString();
            }
            catch
            {
                continue;

            }
            string LotteryNumberResult = "";    // 处理后的结果

            if (String.IsNullOrEmpty(LotteryNumber))
            {
                continue;
            }
            if (LotteryNumber.Split(';').Length != 3)
            {
                continue;
            }

            string PlayType, Content, Way = "";

            try
            {
                PlayType = LotteryNumber.Split(';')[0];
                Content = LotteryNumber.Split(';')[1];
                Way = LotteryNumber.Split(';')[2];
            }
            catch
            {
                continue;
            }

            // 解析Content 
            string[] MatchsAndResult = Content.Substring(1, Content.Length - 1).Split('|');
            // MatchID
            string MatchIDs = "";
            // 胜平负
            string result = "";
            // 周一001
            string MatchNumber = "";

            foreach (string val in MatchsAndResult)
            {
                MatchIDs += val.Split('(')[0] + ",";
                // 去掉尾部')'
                result += val.Split('(')[1].Substring(0, val.Split('(')[1].Length - 1) + ",";
            }

            if (MatchIDs.EndsWith(","))
            {
                MatchIDs = MatchIDs.Substring(0, MatchIDs.Length - 1);
            }
            if (result.EndsWith(","))
            {
                result = result.Substring(0, result.Length - 1);
            }
            if (result.EndsWith(")"))
            {
                result = result.Substring(0, result.Length - 1);
            }

            if (string.IsNullOrEmpty(MatchIDs))
            {
                continue;
            }


            // 通过MatchID 查找内容

            DataTable dtMatch = new DAL.Tables.T_Match().Open("MatchNumber", "id in (" + MatchIDs + ")", "");
            if (dtMatch == null)
            {
                continue;
            }
            if (dtMatch.Rows.Count < 1)
            {
                continue;
            }

            foreach (DataRow mdr in dtMatch.Rows)
            {
                if (mdr["MatchNumber"] != null)
                {
                    MatchNumber += mdr["MatchNumber"].ToString() + ",";
                }
            }

            if (MatchNumber.EndsWith(","))
            {
                MatchNumber = MatchNumber.Substring(0, MatchNumber.Length - 1);
            }

            // 得到投注场次
            BetCount = MatchIDs.Split(',').Length;

            // 拼接LotteryNumber            
            for (int x = 0; x < MatchIDs.Split(',').Length; x++)
            {
                string m = MatchNumber.Split(',')[x];
                string r = GetResult(result.Split(',')[x], PlayType);
                string o = Odds.Split('|')[x];

                LotteryNumberResult += (m + "[" + r + "]" + o + "  ");
            }

            Way = Way.Split(']')[0].Substring(1);
            Way = Way.Substring(0, Way.Length - 1);
            dr["BetWay"] = GetWay(Way);

            dr["LotteryNumber"] = LotteryNumberResult;
            dr["BetCount"] = BetCount + "";

        }

        // 筛选
        if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(datetime))
        {
            DataTable dtt = new DataTable();
            dtt = dtNewUserMessage.Clone();//拷贝框架

            if (userName.Equals("输入用户名"))
            {
                userName = "";
            }
            DataRow[] drs = null;

            drs = dtNewUserMessage.Select("DateTime > '" + datetime + " 0:00:00' and  DateTime < '" + datetime + " 23:59:59' and Name like '%" + userName + "%'");

            for (int i = 0; i < drs.Length; i++)
            {
                dtt.ImportRow((DataRow)drs[i]);//这一句再确认一下。呵呵
            }

            dtNewUserMessage = dtt;
        }
                
        return dtNewUserMessage;
    }
    #region Lottery
    private string GetResult(string Num, string PlayTypeID)
    {
        switch (PlayTypeID)
        {
            case "7201":
                return Get7201(Num);
            case "7202":
                return Get7202(Num);
            case "7203":
                return Get7203(Num);
            case "7204":
                return Get7204(Num);
            default:
                return "";
        }
    }

    /// <summary>
    /// 胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7201(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜";
                break;
            case "2": res = "平";
                break;
            case "3": res = "负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 比分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7202(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "1:0";
                break;
            case "2": res = "2:0";
                break;
            case "3": res = "2:1";
                break;
            case "4": res = "3:0";
                break;
            case "5": res = "3:1";
                break;
            case "6": res = "3:2";
                break;
            case "7": res = "4:0";
                break;
            case "8": res = "4:1";
                break;
            case "9": res = "4:2";
                break;
            case "10": res = "5:0";
                break;
            case "11": res = "5:1";
                break;
            case "12": res = "5:2";
                break;
            case "13": res = "胜其它";
                break;
            case "14": res = "0:0";
                break;
            case "15": res = "1:1";
                break;
            case "16": res = "2:2";
                break;
            case "17": res = "3:3";
                break;
            case "18": res = "平其它";
                break;
            case "19": res = "0:1";
                break;
            case "20": res = "0:2";
                break;
            case "21": res = "1:2";
                break;
            case "22": res = "0:3";
                break;
            case "23": res = "1:3";
                break;
            case "24": res = "2:3";
                break;
            case "25": res = "0:4";
                break;
            case "26": res = "1:4";
                break;
            case "27": res = "2:4";
                break;
            case "28": res = "0:5";
                break;
            case "29": res = "1:5";
                break;
            case "30": res = "2:5";
                break;
            case "31": res = "负其它";
                break;


            default:
                res = "";
                break;
        }

        return res;
    }

    public string GetWay(string way)
    {
        if (way == "AA")
        {
            return "2串1";
        }
        if (way == "AB")
        {
            return "3串1";
        }
        if (way == "AC")
        {
            return "4串1";
        }
        if (way == "AD")
        {
            return "5串1";
        }
        if (way == "AQ")
        {
            return "6串1";
        }
        if (way == "BA")
        {
            return "7串1";
        }
        if (way == "BG")
        {
            return "8串1";
        }
        return "暂无";
    }
    /// <summary>
    /// 总进球
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7203(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "0";
                break;
            case "2": res = "1";
                break;
            case "3": res = "2";
                break;
            case "4": res = "3";
                break;
            case "5": res = "4";
                break;
            case "6": res = "5";
                break;
            case "7": res = "6";
                break;
            case "8": res = "7+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 半全场胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7204(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜胜";
                break;
            case "2": res = "胜平";
                break;
            case "3": res = "胜负";
                break;
            case "4": res = "平胜";
                break;
            case "5": res = "平平";
                break;
            case "6": res = "平负";
                break;
            case "7": res = "负胜";
                break;
            case "8": res = "负平";
                break;
            case "9": res = "负负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    #endregion

}