using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;

using Shove.Database;


/// <summary>
/// ManageClientService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class LotteryPrint : System.Web.Services.WebService
{
    public LotteryPrint()
    {
        //如果使用设计的组件，请取消注释以下行
        //InitializeComponent(); 
    }

    private string GenValidCode()
    {
        System.Random rd = new Random();

        int i = rd.Next(10000000, 99999999);
        int j = (int)(System.Math.Abs(System.Math.Sin((double)i)) * 100000000);
        int k = (int)(System.Math.Abs(System.Math.Cos((double)j)) * 100000000);

        return i.ToString().PadLeft(8, '0') + j.ToString().PadLeft(8, '0') + k.ToString().PadLeft(8, '0');
    }

    private bool CheckValidCode(string ValidCode)
    {
        if (ValidCode.Length != 24)
            return false;

        int i = Shove._Convert.StrToInt(ValidCode.Substring(0, 8), 1);
        int j = (int)(System.Math.Abs(System.Math.Sin((double)i)) * 100000000);
        int k = (int)(System.Math.Abs(System.Math.Cos((double)j)) * 100000000);

        return ((i.ToString().PadLeft(8, '0') + j.ToString().PadLeft(8, '0') + k.ToString().PadLeft(8, '0')) == ValidCode);
    }

    [WebMethod]
    public long GetSiteID(string Url)
    {
        return 1;
    }

    [WebMethod]
    public string GetUserValidCode(long SiteID, string UserName, string UserPassword)
    {
        Users tu = new Users(SiteID);

        tu.Name = UserName;

        string ReturnDescription = "";

        if (tu.GetUserInformationByName(ref ReturnDescription) != 0)
        {
            return "";
        }

        if (tu.Password != UserPassword)
        {
            return "";
        }

        if (!tu.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryBuy)))
        {
            return "";
        }

        return GenValidCode();
    }

    [WebMethod]
    public string GetUserID(long SiteID, string UserName, string ValidCode)
    {
        if (!CheckValidCode(ValidCode))
        {
            return "非法用户调用";
        }

        Users tu = new Users(SiteID);
        tu.Name = UserName;

        string ReturnDescription = "";

        if (tu.GetUserInformationByName(ref ReturnDescription) != 0)
        {
            return "";
        }

        return tu.ID.ToString();
    }

    [WebMethod]
    public string GetUserRightsList(long SiteID, string UserName, string ValidCode)
    {
        if (!CheckValidCode(ValidCode))
        {
            return "非法用户调用";
        }

        Users tu = new Users(SiteID);
        tu.Name = UserName;

        string ReturnDescription = "";

        if (tu.GetUserInformationByName(ref ReturnDescription) != 0)
        {
            return "";
        }

        if (!tu.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryBuy)))
        {
            return "";
        }

        return "True";
    }

    [WebMethod]
    public string GetUserLotteryBuyRightsList(long SiteID, string UserName, string ValidCode)
    {
        if (!CheckValidCode(ValidCode))
        {
            return "非法用户调用";
        }

        Users tu = new Users(SiteID);

        tu.Name = UserName;

        string ReturnDescription = "";

        if (tu.GetUserInformationByName(ref ReturnDescription) != 0)
        {
            return "";
        }

        if (!tu.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryBuy)))
        {
            return "";
        }

        return "True";
    }

    [WebMethod]
    public string GetServerDateTime()
    {
        return DateTime.Now.ToString();
    }

    [WebMethod]
    public string GetServerDateTimeEncryptString()
    {
        return Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), DateTime.Now.ToString());
    }

    [WebMethod]
    public string GetLottery(long SiteID, string ValidCode)
    {
        if (!CheckValidCode(ValidCode))
        {
            return "";
        }

        string UseLotteryList = new Sites()[SiteID].UseLotteryList;

        DataTable dt = new DAL.Views.V_SchemeCount().Open("*", "[ID] in (select [ID] from T_Lotteries where [ID] in (" + UseLotteryList + ") and PrintOutType = 0)", "[Order]");

        if (dt == null)
        {
            return "";
        }

        DataSet ds = new DataSet();
        dt.TableName = "Lottery";
        ds.Tables.Add(dt);

        return ds.GetXml();
    }

    [WebMethod]
    public string GetIsuse(long SiteID, int LotteryID, string ValidCode)
    {
        if (!CheckValidCode(ValidCode))
        {
            return "";
        }

        DataTable dt = MSSQL.Select("select top 1 [id], Name, StartTime, EndTime from T_Isuses where LotteryID = " + LotteryID.ToString() + " and GetDate() Between StartTime and EndTime order by StartTime");
        if (dt == null)
            return "";

        DataSet ds = new DataSet();
        dt.TableName = "Isuse";
        ds.Tables.Add(dt);

        return ds.GetXml();
    }

    [WebMethod]
    public string GetScheme(long IsuseID, string ValidCode)
    {
        if (!CheckValidCode(ValidCode))
        {
            return "";
        }

        DataTable dt = new DAL.Views.V_SchemeSchedulesWithQuashed().Open("id,DateTime,SchemeNumber,Title,InitiateName,PlayTypeName,LotteryNumber,Multiple,Money,AssureMoney,LotteryID,IsuseID,LotteryName,Code", "IsuseID = " + IsuseID.ToString() + "and Schedule >= 100 and QuashStatus = 0 and Buyed = 0 and LotteryNumber <> ''", " [Money] desc");

        if ((dt == null) || dt.Rows.Count < 1)
        {
            return "";
        }

        DataSet ds = new DataSet();
        dt.TableName = "Scheme";
        ds.Tables.Add(dt);

        return ds.GetXml();
    }

    [WebMethod]
    public string LotteryBuy(long SchemeID, string LotteryCode, long SiteID, long UserID, string ValidCode)
    {
        if (!CheckValidCode(ValidCode))
        {
            new Log("System").Write("非法用户调用");

            return "非法用户调用";
        }

        if (LotteryCode == "")
        {
            new Log("System").Write("请电询");

            LotteryCode = "请电询";
        }

        int ReturnValue = 0;
        string ReturnDescription = "";

        if (DAL.Procedures.P_SchemePrintOut(SiteID, SchemeID, UserID, PrintOutTypes.Local, LotteryCode, true, ref ReturnValue, ref ReturnDescription) < 0)
        {
            new Log("System").Write("数据读写错误");

            return "数据读写错误";
        }

        if (ReturnDescription != "")
        {
            new Log("System").Write(ReturnDescription);

            return ReturnDescription;
        }

        return "";
    }
}