using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Competences 的摘要说明
/// </summary>
public class Competences
{
    #region 权限常量

    /// <summary>
    /// 超级管理权限，拥有对网站的全部控制权。
    /// </summary>
    public const string Administrator = "Administrator";
    /// <summary>
    /// 管理网站会员的权限、控制、设置等权限。
    /// </summary>
    public const string Competence = "Competence";
    /// <summary>
    /// 填充网站的各种内容的权限。
    /// </summary>
    public const string FillContent = "FillContent";
    /// <summary>
    /// 编辑、发布网站的各种新闻资讯栏目；编辑网站公告的权限。
    /// </summary>
    public const string EditNews = "EditNews";
    /// <summary>
    /// 查询、修改会员信息的权限。
    /// </summary>
    public const string MemberManagement = "MemberManagement";
    /// <summary>
    /// 会员账户充值的权限。
    /// </summary>
    public const string AddMoney = "AddMoney";
    /// <summary>
    /// 接受、处理会员提款的权限。
    /// </summary>
    public const string DistillMoney = "DistillMoney";
    /// <summary>
    /// 查阅会员提问、反馈答复会员资讯、解决客户问题的权限。
    /// </summary>
    public const string MemberService = "MemberService";
    /// <summary>
    /// 发送站内短消息、向会员群发邮件、发送手机短信的权限。
    /// </summary>
    public const string SendMessage = "SendMessage";
    /// <summary>
    /// 管理系统日志的权限。
    /// </summary>
    public const string Log = "Log";
    /// <summary>
    /// 管理彩票销售期号、方案置顶、撤单等权限。
    /// </summary>
    public const string LotteryIsuseScheme = "LotteryIsuseScheme";
    /// <summary>
    /// 销售彩票、出票打印的权限。
    /// </summary>
    public const string LotteryBuy = "LotteryBuy";
    /// <summary>
    /// 彩票开奖、奖金派发的权限。
    /// </summary>
    public const string LotteryWin = "LotteryWin";
    /// <summary>
    /// 网站各参数设置的高级权限。
    /// </summary>
    public const string Options = "Options";
    /// <summary>
    /// 网站财务报表查询的权限。
    /// </summary>
    public const string Finance = "Finance";
    /// <summary>
    /// CPS内容管理，拥有CPS资讯和商家管理。
    /// </summary>
    public const string CpsManage = "CpsManage";

    /// <summary>
    /// 查询权限
    /// </summary>
    public const string QueryData = "QueryData";

    /// <summary>
    /// 积分系统
    /// </summary>
    public const string Score = "Score";
    #endregion

    public Users User;

    public string CompetencesList
    {
        get
        {
            if ((User == null) || (User.ID < 1))
            {
                throw new Exception("没有初始化 Competences 类的 User 变量");
            }

            return DAL.Functions.F_GetUserCompetencesList(User.ID);
        }
    }

    public Competences()
    {
        User = null;
    }

    public Competences(Users user)
    {
        User = user;
    }

    public bool this[string CompetenceName]
    {
        get
        {
            if ((User == null) || (User.ID < 1))
            {
                throw new Exception("没有初始化 Competences 类的 User 变量");
            }

            return (DAL.Functions.F_GetUserCompetencesList(User.ID).IndexOf("[" + CompetenceName + "]") >= 0);
        }
    }

    // 设置用户权限
    public int SetUserCompetences(string CompetencesList, string GroupsList, ref string ReturnDescription)
    {
        if ((User == null) || (User.ID < 1))
        {
            throw new Exception("没有初始化 Competences 类的 User 变量");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_SetUserCompetences(User.Site.ID, User.ID, CompetencesList, GroupsList, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        return 0;
    }

    // 是否拥有权限列表需要的权限
    public bool IsOwnedCompetences(string RequestCompetencesList)
    {
        if ((User == null) || (User.ID < 1))
        {
            throw new Exception("没有初始化 Competences 类的 User 变量");
        }

        string UserCompetencesList = CompetencesList;

        if (UserCompetencesList.IndexOf("[" + Competences.Administrator + "]") >= 0)    // 拥有超级权限
        {
            return true;
        }

        RequestCompetencesList = RequestCompetencesList.Trim();

        if (RequestCompetencesList == "")
        {
            return true;
        }

        RequestCompetencesList = RequestCompetencesList.Replace("][", ",");
        RequestCompetencesList = RequestCompetencesList.Substring(1, RequestCompetencesList.Length - 2);

        string[] strs = RequestCompetencesList.Split(',');

        bool HaveThisCompetences = false;

        foreach (string str in strs)
        {
            if ((UserCompetencesList.IndexOf("[" + str + "]") >= 0) && (!HaveThisCompetences))
            {
                HaveThisCompetences = true;
            }
        }

        return HaveThisCompetences;
    }

    // 构建需要的权限列表字串
    public static string BuildCompetencesList(params string[] CompetenceList)
    {
        string Result = "";

        foreach (string str in CompetenceList)
        {
            Result += "[" + str + "]";
        }

        return Result;
    }
}
