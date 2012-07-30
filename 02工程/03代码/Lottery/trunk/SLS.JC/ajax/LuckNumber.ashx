<%@ WebHandler Language="C#" Class="LuckNumber" %>

using System;
using System.Web;
using System.Data;

public class LuckNumber : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        int LotteryID = 0;

        if (!string.IsNullOrEmpty(context.Request["LotteryID"]))
        {
            LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.FilteSqlInfusion(context.Request["LotteryID"]), 0);
        }

        string Type = "";

        if (!string.IsNullOrEmpty(context.Request["Type"]))
        {
            Type = Shove._Web.Utility.FilteSqlInfusion(context.Request["Type"]);
        }

        string Name = "";

        if (!string.IsNullOrEmpty(context.Request["Name"]))
        {
            Name = Shove._Web.Utility.FilteSqlInfusion(context.Request["Name"]);
        }

        if (LotteryID == 0 || string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(Name))
        {
            context.Response.Write("{\"message\": \"参数错误，请重新发起请求！\"}");
            context.Response.End();
        }

        string Key = "Home_Room_Buy_GenerateLuckLotteryNumber" + LotteryID.ToString();

        if (Type == "3")
        {
            try
            {
                DateTime time = Convert.ToDateTime(Name);
                Name = time.ToString("yyyy-MM-dd");

                if (time > DateTime.Now)
                {
                    context.Response.Write("{\"message\": \"出生日期不能超过当前日期！\"}");
                    context.Response.End();
                }
            }
            catch
            {
                context.Response.Write("{\"message\": \"日期格式不正确！\"}");
                context.Response.End();
            }
        }

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Tables.T_LuckNumber().Open("", "datediff(d,getdate(),DateTime)=0 and LotteryID=" + LotteryID.ToString() + "", "");

            Shove._Web.Cache.SetCache(Key, dt, 3600);
        }

        string LotteryNumber = "";

        DataRow[] dr = dt.Select("Type=" + Type + " and Name='" + Name + "'");

        if (dr != null && dr.Length > 0)
        {
            LotteryNumber = dr[0]["LotteryNumber"].ToString();
        }
        else
        {
            if (LotteryID == 3 || LotteryID == 63 || LotteryID == 64 || LotteryID == 9)
            {
                LotteryNumber = new SLS.Lottery()[LotteryID].BuildNumber(1);
            }
            else if (LotteryID == 39)
            {
                LotteryNumber = new SLS.Lottery()[LotteryID].BuildNumber(5, 2, 1);
            }

            DAL.Tables.T_LuckNumber ln = new DAL.Tables.T_LuckNumber();

            ln.LotteryID.Value = LotteryID;
            ln.LotteryNumber.Value = LotteryNumber;
            ln.Name.Value = Name;
            ln.Type.Value = Type;

            ln.Insert();
            ln.Delete("datediff(d,DateTime,getdate())>0");

            Shove._Web.Cache.ClearCache(Key);
        }

        context.Response.Write("{\"message\": \"1\", \"LotteryNumber\": \"" + LotteryNumber + "|" + FormatLuckLotteryNumber(LotteryID, LotteryNumber) + "\"}");
        context.Response.End();
    }

    private string FormatLuckLotteryNumber(int LotteryID, string LotteryNumber)
    {
        if (string.IsNullOrEmpty(LotteryNumber))
        {
            return "";
        }

        LotteryNumber = LotteryNumber.Replace(" + ", " ");

        return LotteryNumber;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}