<%@ WebHandler Language="C#" Class="Buy_Handler" %>

using System;
using System.Web;
using System.Text;
using Microsoft.JScript;
using System.Text.RegularExpressions;
using System.Web.SessionState;

using Shove.Database;

/// <summary>
/// 单场的投注处理程序
/// </summary>
public class Buy_Handler : IHttpHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        Buy(context);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 购买彩票
    /// </summary>
    private void Buy(HttpContext context)
    {

        Users _User = Users.GetCurrentUser(1);
        if (_User == null)
        {
            context.Response.Write("您的登录信息丢失，请重新登录。");
            context.Response.End();
        }

        #region 获取表单数据

        string lotId = Shove._Web.Utility.GetRequest(context, "lotid");                         //彩票编号
        string playid = Shove._Web.Utility.GetRequest(context, "playid");                     //玩法
        string ggtypeid = Shove._Web.Utility.GetRequest(context, "ggtypeid");                   //过关方式（M串N）

        string codes = Shove._Web.Utility.GetRequest(context, "codes");                         //投注内容
        string zhushu = Shove._Web.Utility.GetRequest(context, "zhushu");                       //基本注数
        string multiple = Shove._Web.Utility.GetRequest(context, "beishu");                   //投注倍数
        string totalMoney = Shove._Web.Utility.GetRequest(context, "totalmoney");               //投注总金额

        double SchemeBonusScale = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest(context, "tb_SchemeBonusScale"), 4);               //佣金
        int Share = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest(context, "tb_Share"), 1);               //总份数
        double AssureMoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest(context, "AssureMoney"), 0);               //保底金额
        int BuyShare = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest(context, "tb_BuyShare"), 1);               //购买份数
        short SecrecyL = Shove._Convert.StrToShort(Shove._Web.Utility.GetRequest(context, "SecrecyL"), 0);               //方案保密等级
        string Title = Shove._Web.Utility.GetRequest(context, "tb_Title");                         //方案标题

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
        short SecrecyLevel = 0;         //彩票单价

        double TotalBuyMoney = 0;       //总购买金（认购）

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
            context.Response.Write("输入有错误，请仔细检查。01");
            context.Response.End();
        }


        //验证彩种和玩法
        if (LotteryID != 73 || (PlayTypeID != 7301 && PlayTypeID != 7302 && PlayTypeID != 7303 && PlayTypeID != 7304))
        {
            context.Response.Write("输入有错误，请仔细检查。03");
            context.Response.End();
        }

        string BuyNumber = codes.Trim().Split(';')[1].ToString();

        
        
        
        string FireTeam = BuyNumber.Substring(1, BuyNumber.IndexOf('(') - 1).Trim();

        System.Data.DataTable dt = Shove.Database.MSSQL.Select(Shove._Web.WebConfig.GetAppSettingsString("ConnectionString"), "SELECT StopSellTime from T_PassRateBasket where MatchID=" + FireTeam, null);

        if ((dt == null) || (dt.Rows.Count == 0))
        {
            context.Response.Write("数据读取错误,请稍后再访问!");
            context.Response.End();
        }

        if (Shove._Convert.StrToDateTime(dt.Rows[0]["StopSellTime"].ToString(), DateTime.Now.AddDays(-1).ToString()).CompareTo(DateTime.Now) < 0)
        {
            context.Response.Write("您所选择的场次里面包含了已截止场次, 请重新选择!");
            context.Response.End();
        }

        //验证方案基本信息
        if (BaseCount < 1 || Multiple < 1 || SumMoney < 2 || SumNum < 1)
        {
            context.Response.Write("输入有错误，请仔细检查。05");
            context.Response.End();
        }

        if (SumMoney != BaseCount * Multiple * Price)
        {
            context.Response.Write("输入有错误，请仔细检查。06");
            context.Response.End();
        }

        long IsuseID = 0;

        dt = new DAL.Tables.T_Isuses().Open("ID", "LotteryID= 73", "EndTime desc");

        if (dt == null)
        {
            context.Response.Write("数据读取错误,请稍后再访问!");
            context.Response.End();
        }

        if (dt.Rows.Count < 1)
        {
            context.Response.Write("期号不存在，请添加期号！");
            context.Response.End();
        }

        IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), 0);

        //余额判断
        if (TotalBuyMoney > _User.Balance)
        {
            DAL.Tables.T_AlipayBuyTemp tbp = new DAL.Tables.T_AlipayBuyTemp();

            tbp.SiteID.Value = 1;
            tbp.UserID.Value = -1;
            tbp.Money.Value = SumMoney;
            tbp.HandleResult.Value = 0;
            tbp.IsCoBuy.Value = true;
            tbp.LotteryID.Value = LotteryID;
            tbp.IsuseID.Value = IsuseID;
            
            tbp.PlayTypeID.Value = PlayTypeID;
            tbp.Title.Value = Title;
            
            tbp.Description.Value = "";
            tbp.LotteryNumber.Value = codes;
            tbp.UpdateloadFileContent.Value = "";
            tbp.Multiple.Value = Multiple;

            tbp.BuyMoney.Value = (SumMoney / Share * BuyShare);
            tbp.SumMoney.Value = SumMoney;
            tbp.AssureMoney.Value = AssureMoney;


            tbp.Share.Value = Share;
            tbp.BuyShare.Value = BuyShare;
            tbp.AssureShare.Value = (AssureMoney / (SumMoney / Share));
            tbp.OpenUsers.Value = "";
            tbp.SecrecyLevel.Value = SecrecyL;

            long Result = tbp.Insert();

            if (Result < 0)
            {
                new Log("System").Write("T_AlipayBuyTemp 数据库读写错误。");
            }

            context.Response.Write("BuyID=" + Result.ToString());
            context.Response.End();
        }

        if (TotalBuyMoney > PF.SchemeMaxBettingMoney)
        {
            context.Response.Write("投注金额不能大于" + PF.SchemeMaxBettingMoney.ToString() + "，谢谢！");
            context.Response.End();
        }

        #endregion

        #region 对彩票号码进行分析，判断注数

        SLS.Lottery slsLottery = new SLS.Lottery();
        string[] t_lotterys = SplitLotteryNumber(codes);

        if ((t_lotterys == null) || (t_lotterys.Length < 1))
        {
            context.Response.Write("选号发生异常，请重新选择号码投注，谢谢。(-694)");
            context.Response.End();
        }

        int ValidNum = 0;

        foreach (string str in t_lotterys)
        {
            string Number = slsLottery[73].AnalyseScheme(str, PlayTypeID);

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
            context.Response.Write("选号发生异常，请重新选择号码投注，谢谢11。");
            context.Response.End();
        }

        #endregion

        #region 投注区域
        
        //第二步：保存到数据库

        string ReturnDescription = "";
        long SchemeID = _User.InitiateScheme(IsuseID, PlayTypeID, Title == "" ? "(无标题)" : Title, "", codes, "", Multiple, SumMoney, AssureMoney, Share, BuyShare, "", SecrecyL, SchemeBonusScale * 0.01, ref ReturnDescription);
        if (SchemeID < 0)
        {
            context.Response.Write("方案提交失败。错误描述：" + ReturnDescription);
            context.Response.End();
        }

        context.Response.Write("true");

        #endregion
    }

    private string[] SplitLotteryNumber(string Number)
    {
        string[] s = Number.Split('\n');
        if (s.Length == 0)
            return null;
        for (int i = 0; i < s.Length; i++)
            s[i] = s[i].Trim();
        return s;
    }

}