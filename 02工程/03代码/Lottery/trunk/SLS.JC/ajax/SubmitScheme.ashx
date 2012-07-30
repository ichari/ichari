<%@ WebHandler Language="C#" Class="SubmitScheme" %>

using System;
using System.Web;
using System.Data;
using System.Text;

public class SubmitScheme : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";

        Users _User = Users.GetCurrentUser(1);
        
        if (_User == null)
        {
            context.Response.Write("{\"message\": \"您的登录信息丢失，请重新登录。\"}");
            context.Response.End();
        }

        #region 获取表单数据

        string lotId = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "lotid"));                         //彩票编号
        string playid = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "playid"));                     //玩法

        string codes = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "codes")).Replace("|", "+");                         //投注内容
        string zhushu = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "zhushu"));                       //基本注数
        string multiple = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "beishu"));                   //投注倍数
        string totalMoney = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "totalmoney"));               //投注总金额
        double SchemeBonusScale = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest(context, "bScalec"), 0.04);               //佣金
        int Share = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest(context, "Share"), 1);               //总份数
        double AssureMoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest(context, "AssureMoney"), 0);               //保底金额
        int BuyShare = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest(context, "BuyShare"), 1);               //购买份数
        short SecrecyL = Shove._Convert.StrToShort(Shove._Web.Utility.GetRequest(context, "SecrecyL"), 0);               //方案保密等级
        string Title = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "Title"));                         //方案标题
        long IsuseID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest(context, "IsuseID"), 0);
        DateTime EndTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest(context, "EndTime"), DateTime.Now.AddHours(-1).ToString());

        #endregion
        
        #region 数据验证区

        //************局部变量

        //彩种信息
        int LotteryID = 0;              //彩种
        int PlayTypeID = 0;             //玩法
        double Price = 2.00;                  //彩票单价

        //方案信息
        int BaseCount = 0;              //投注基本注数
        int Multiple = 0;               //投注倍数，       
        int SumNum = 0;                 //总的注数（基本注数*倍数）
        double SumMoney = 0;            //方案总金额

        //验证数据格式是否正确
        try
        {
            LotteryID = int.Parse(lotId);
            PlayTypeID = int.Parse(playid);

            BaseCount = int.Parse(zhushu);
            Multiple = int.Parse(multiple);
            SumNum = BaseCount;
            SumMoney = double.Parse(totalMoney);
        }
        catch
        {
            context.Response.Write("{\"message\": \"输入有错误，请仔细检查。" + LotteryID.ToString() + "!"+ PlayTypeID.ToString() +"!"+ BaseCount.ToString() +"|"+ multiple.ToString() +""+ SumNum.ToString() +"|"+ SumMoney.ToString() +"\"}");
            context.Response.End();
        }

        //验证方案基本信息
        if (BaseCount < 1 || Multiple < 1 || SumMoney < 2 || SumNum < 1)
        {
            context.Response.Write("{\"message\": \"输入有错误，请仔细检查。\"}");
            context.Response.End();
        }

        if (PlayTypeID == 3903 || PlayTypeID == 3904 || PlayTypeID == 3908)
        {
            Price = 3;
        }

        if (SumMoney != BaseCount * Multiple * Price)
        {
            context.Response.Write("{\"message\": \"输入有错误，请仔细检查。\"}");
            context.Response.End();
        }

        //余额判断
        if (SumMoney > _User.Balance)
        {
            //SaveDataForAliBuy();
            context.Response.Write("{\"message\": \"您的余额不足，请充值后再投注！\"}");
            context.Response.End();
        }

        if (SumMoney > PF.SchemeMaxBettingMoney)
        {
            context.Response.Write("{\"message\": \"投注金额不能大于" + PF.SchemeMaxBettingMoney.ToString() + "，谢谢！\"}");
            context.Response.End();
        }

        if (DateTime.Now.CompareTo(EndTime) >= 0)
        {
            context.Response.Write("{\"message\": \"投注已截止，请选择其它期号进行投注！\"}");
            context.Response.End();
        }

        #endregion

        #region 对彩票号码进行分析，判断注数

        if (string.IsNullOrEmpty(codes) && (LotteryID != 74) && (LotteryID != 75) && (LotteryID != 15) && (LotteryID != 2))
        {
            context.Response.Write("{\"message\": \"投注号码不能为空，请重新选择投注号码，谢谢！\"}");
            context.Response.End();
        }

        if (!string.IsNullOrEmpty(codes))
        {
            SLS.Lottery slsLottery = new SLS.Lottery();
            string[] t_lotterys = SplitLotteryNumber(codes);

            if ((t_lotterys == null) || (t_lotterys.Length < 1))
            {
                context.Response.Write("{\"message\": \"选号发生异常，请重新选择号码投注，谢谢。\"}");
                context.Response.End();
            }

            int ValidNum = 0;

            foreach (string str in t_lotterys)
            {
                string Number = slsLottery[LotteryID].AnalyseScheme(str, PlayTypeID);

                if (string.IsNullOrEmpty(Number))
                {
                    continue;
                }

                string[] str_s = Number.Split('|');

                if (str_s == null || str_s.Length < 1)
                {
                    continue;
                }

                ValidNum += Shove._Convert.StrToInt(str_s[str_s.Length - 1], 0);
            }

            if (ValidNum != SumNum)
            {
                new Log("aaa").Write(codes);
                
                context.Response.Write("{\"message\": \"选号发生异常，请重新选择号码投注，谢谢。\"}");
                context.Response.End();
            }
        }

        #endregion

        if (!string.IsNullOrEmpty(codes) && (LotteryID == 74 || LotteryID == 75 || LotteryID == 15))
        {
            int SNumberCount = 0;
            int PNumberCount = 0;
            int FNumberCount = 0;
            string t_str = "";
            string Temp_str = "";
            int Count = 14;

            string[] t_Number = codes.Split('*');

            StringBuilder sb = new StringBuilder();

            foreach (string str in t_Number)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }

                t_str = str;

                if (LotteryID == 15)
                {
                    Count = 12;
                }

                for (int i = 0; i < Count; i++)
                {
                    SNumberCount = 0;
                    PNumberCount = 0;
                    FNumberCount = 0;
                    
                    if (t_str.Substring(0, 1).Equals("("))
                    {
                        Temp_str = t_str.Substring(1, t_str.IndexOf(")") - 1);

                        t_str = t_str.Substring(t_str.IndexOf(")") + 1);
                    }
                    else
                    {
                        Temp_str = t_str.Substring(0, 1);

                        t_str = t_str.Substring(1);
                    }

                    if (Temp_str.IndexOf("3") >= 0)
                    {
                        SNumberCount += 1;
                    }

                    if (Temp_str.IndexOf("1") >= 0)
                    {
                        PNumberCount += 1;
                    }

                    if (Temp_str.IndexOf("0") >= 0)
                    {
                        FNumberCount += 1;
                    }

                    if (LotteryID == 15)
                    {
                        sb.AppendLine("Update T_IsuseForLCBQC set winScale = isnull(winScale, 0) + " + SNumberCount.ToString() + ", drawScale = isnull(drawScale, 0) +" + PNumberCount.ToString() + ", lostScale = isnull(lostScale, 0) +" + FNumberCount.ToString() + " where IsuseID = " + IsuseID.ToString() + " and No= " + i.ToString() + " ;");
                    }
                    else
                    {
                        sb.AppendLine("Update T_IsuseForSFC set winScale = isnull(winScale, 0) + " + SNumberCount.ToString() + ", drawScale = isnull(drawScale, 0) +" + PNumberCount.ToString() + ", lostScale = isnull(lostScale, 0) +" + FNumberCount.ToString() + " where IsuseID = " + IsuseID.ToString() + " and No= " + i.ToString() + " ;");
                    }
                }
            }

            Shove.Database.MSSQL.ExecuteSQLScript(sb.ToString());
        }

        if (!string.IsNullOrEmpty(codes) && (LotteryID == 2))
        {
            int First = 0;
            int Second = 0;
            int three = 0;
            int Fourth = 0;
            string t_str = "";
            string Temp_str = "";

            string[] t_Number = codes.Split('*');

            StringBuilder sb = new StringBuilder();

            foreach (string str in t_Number)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }

                t_str = str;

                for (int i = 0; i < 8; i++)
                {
                    First = 0;
                    Second = 0;
                    three = 0;
                    Fourth = 0;

                    if (t_str.Substring(0, 1).Equals("("))
                    {
                        Temp_str = t_str.Substring(1, t_str.IndexOf(")") - 1);

                        t_str = t_str.Substring(t_str.IndexOf(")") + 1);
                    }
                    else
                    {
                        Temp_str = t_str.Substring(0, 1);

                        t_str = t_str.Substring(1);
                    }

                    if (Temp_str.IndexOf("0") >= 0)
                    {
                        First += 1;
                    }

                    if (Temp_str.IndexOf("1") >= 0)
                    {
                        Second += 1;
                    }

                    if (Temp_str.IndexOf("2") >= 0)
                    {
                        three += 1;
                    }

                    if (Temp_str.IndexOf("3") >= 0)
                    {
                        Fourth += 1;
                    }

                    sb.AppendLine("Update T_IsuseForJQC set winScale = isnull(First, 0) + " + First.ToString() + ", Second = isnull(Second, 0) +" + Second.ToString() + ", three = isnull(three, 0) +" + three.ToString() + ", Fourth = isnull(Fourth, 0) +" + Fourth.ToString() + " where IsuseID = " + IsuseID.ToString() + " and No= " + i.ToString() + " ;");
                }
            }

            Shove.Database.MSSQL.ExecuteSQLScript(sb.ToString());
        }

        #region 投注区域

        DataTable dt = new DAL.Tables.T_Isuses().Open("ID", "ID= " + IsuseID + " and LotteryID=" + LotteryID.ToString(), "");

        if (dt == null)
        {
            context.Response.Write("{\"message\": \"数据读取错误,请稍后再访问!\"}");
            context.Response.End();
        }

        if (dt.Rows.Count < 1)
        {
            context.Response.Write("{\"message\": \"期号不存在，请添加期号！\"}");
            context.Response.End();
        }

        //第二步：保存到数据库

        string ReturnDescription = "";
        long SchemeID = _User.InitiateScheme(IsuseID, PlayTypeID, Title == "" ? "(无标题)" : Title, "", codes.Replace('#','\n'), "", Multiple, SumMoney, AssureMoney, Share, BuyShare, "", SecrecyL, SchemeBonusScale * 0.01, ref ReturnDescription);
        if (SchemeID < 0)
        {
            context.Response.Write("{\"message\": \"方案提交失败。错误描述：" + ReturnDescription + "\"}");
            context.Response.End();
        }

        context.Response.Write("{\"message\": \"" + SchemeID.ToString() + "\"}");
        context.Response.End();

        #endregion
    }

    private string[] SplitLotteryNumber(string Number)
    {
        string[] s = Number.Split('*');
        if (s.Length == 0)
            return null;
        for (int i = 0; i < s.Length; i++)
            s[i] = s[i].Trim();
        return s;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}