using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Challenge_ChallengeSchemes : System.Web.UI.Page
{

    private string SchemesID = "";
    protected string UserName = "";  // 用户
    protected string BetTime = "";  // 日期
    protected string BetCount = ""; //注数
    protected string Way = ""; //f过关方式
    protected string WinMoney = "";// 预测奖金
    protected StringBuilder sbHtml = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SchemesID = Shove._Web.Utility.GetRequest("SchemesID");
            if (string.IsNullOrEmpty(SchemesID))
            {
                return;
            }

            BindSchemesDetails();
        }
    }

    private void BindSchemesDetails()
    {        
        string sql = string.Format("select u.[Name], s.[DateTime],s.[LotteryNumber],s.[Odds] from T_ChallengeScheme as s ,T_Users as u where s.ID = {0} and u.ID = s.InitiateUserId",SchemesID);
        DataTable dtScheme = Shove.Database.MSSQL.Select(sql);
        if (dtScheme == null)
        {
            Shove._Web.JavaScript.Alert(this.Page,"无法获取此方案");
            return;
        }

        if (dtScheme.Rows.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "没有可读取的方案");
            return;
        }

        UserName = dtScheme.Rows[0]["Name"] + "";
        BetTime = Shove._Convert.StrToDateTime(dtScheme.Rows[0]["DateTime"] + "", DateTime.Now.ToString()).ToString("yyyy-MM-dd");
        // 计算预测奖金
        string[] win = dtScheme.Rows[0]["Odds"].ToString().Split('|');

        WinMoney = (Shove._Convert.StrToDouble(win[0],1) * Shove._Convert.StrToDouble(win[1],1) * 2).ToString("F2") + "";
        
        // 开始解析LotteyrNumber

        
        // 号码
        string LotteryNumber = dtScheme.Rows[0]["LotteryNumber"] + "";


        if (String.IsNullOrEmpty(LotteryNumber))
        {
            Shove._Web.JavaScript.Alert(this.Page, "解析此方案失败");
            return;
        }
        if (LotteryNumber.Split(';').Length != 3)
        {
            Shove._Web.JavaScript.Alert(this.Page, "解析此方案失败");
            return;
        }

        string PlayType, Content;

        try
        {
            PlayType = LotteryNumber.Split(';')[0];
            Content = LotteryNumber.Split(';')[1];
            Way = LotteryNumber.Split(';')[2];
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "解析此方案失败");
            return;
        }

        // 解析Content 
        string[] MatchsAndResult = Content.Substring(1, Content.Length - 1).Split('|');
        // MatchID
        string MatchIDs = "";
        // 胜平负
        string result = "";
        // 周一001

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
           Shove._Web.JavaScript.Alert(this.Page, "解析此方案失败");
            return;
        }

        // 得到投注场次
        BetCount = MatchIDs.Split(',').Length + "";
        Way = Way.Split(']')[0].Substring(1);
        Way = GetWay(Way.Substring(0, Way.Length - 1));

        // 通过MatchID 查找内容

        DataTable dtMatch =  Shove.Database.MSSQL.Select("select MatchNumber,Game,GameColor,MatchDate,MainTeam,MainLoseball,GuestTeam,Win,Flat,Lose from T_PassRate where MatchID in ("+ MatchIDs + ")");
        
        // 解决一边一条数据的问题
        DataTable dtMatchNew = new DataTable();

        if (dtMatch == null)
        {
            dtMatch = Shove.Database.MSSQL.Select("select MatchNumber,Game,GameColor,MatchDate,MainTeam,MainLoseball,GuestTeam,'' as Win,'' as Flat,'' as Lose from T_Match where ID in (" + MatchIDs + ")");
        }

        if (dtMatch.Rows.Count == 0)
        {
            dtMatch = Shove.Database.MSSQL.Select("select MatchNumber,Game,GameColor,MatchDate,MainTeam,MainLoseball,GuestTeam,'' as Win,'' as Flat,'' as Lose from T_Match where ID in (" + MatchIDs + ")");
        }
        if (dtMatch.Rows.Count == 1)
        { // 一边一条
            dtMatch = Shove.Database.MSSQL.Select("select MatchNumber,Game,GameColor,MatchDate,MainTeam,MainLoseball,GuestTeam,'' as Win,'' as Flat,'' as Lose from T_Match where ID in (" + MatchIDs + ")");
        }

        if (dtMatch == null && dtMatchNew == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "此方案记录不存在");

            return;
        }
        if (dtMatch.Rows.Count < 1 && dtMatchNew.Rows.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "此方案记录不存在");
            return;
        }
        // 得到胜平负
      //  GetResult(result.Split(',')[x], PlayType)
       
        int index = 0;
        
        bool IsNull = false;

        string[] r = result.Split(',');

        foreach (DataRow mdr in dtMatch.Rows)
        {
            if (string.IsNullOrEmpty(mdr["Win"].ToString()) && string.IsNullOrEmpty(mdr["Flat"].ToString()) && string.IsNullOrEmpty(mdr["Lose"].ToString()))
            {
                IsNull = true;
            }

            string s, p, fu;

            s = mdr["Win"].ToString(); p = mdr["Lose"].ToString(); fu = mdr["Flat"].ToString();

            if (r[index].Equals("1"))
            {
                if (IsNull)
                {
                    s = win[index];
                }
            }
            else if (r[index].Equals("2"))
            {
                if (IsNull)
                {
                    p = win[index];
                }
            }
            else if (r[index].Equals("3"))
            {
                if (IsNull)
                {
                    fu = win[index];
                }
            }

            // string[] win  

            s = string.IsNullOrEmpty(s) ? "&nbsp;" : s;
            p = string.IsNullOrEmpty(p) ? "&nbsp;" : p;
            fu = string.IsNullOrEmpty(fu) ? "&nbsp;" : fu;

            sbHtml.AppendLine("<tr class=\"sk1\">")
                .AppendLine("<td>" + mdr["MatchNumber"] + "</td>")
                .AppendLine("<td style=\"background-color:" + mdr["GameColor"] + ";color:White;\">" + mdr["Game"] + "</td>")
                .AppendLine("<td><span class=\"o11\">" + Shove._Convert.StrToDateTime(mdr["MatchDate"].ToString(), DateTime.Now.ToString()).ToString("MM-dd hh:mm") + "</span></td>")
                .AppendLine("<td>" + mdr["MainTeam"] + "</td>")
                .AppendLine("<td>" + mdr["MainLoseball"] + "</td>")
                .AppendLine("<td>" + mdr["GuestTeam"] + "</td>")
                .AppendLine("<td>")
                .AppendLine("<span style=\"font-weight:bold\">" + s + "</span></td>")
                .AppendLine("<td>")
                .AppendLine("<span style=\"font-weight:bold\">" + p + "</span></td>")
                .AppendLine("<td>")
                .AppendLine("<span style=\"font-weight:bold\">" + fu + "</span></td>")
                .AppendLine("<td><b style=\"color:Red\"></b></td>")
                .AppendLine("</tr>");
            index++;
        }
        
        sbHtml.AppendLine("</table>");        
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
    #endregion

}
