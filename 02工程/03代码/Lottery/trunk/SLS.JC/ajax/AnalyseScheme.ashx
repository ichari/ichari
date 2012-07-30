<%@ WebHandler Language="C#" Class="AnalyseScheme" %>

using System;
using System.Web;

public class AnalyseScheme : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        string Content = "";

        if (!string.IsNullOrEmpty(context.Request["Content"]))
        {
            Content = Shove._Web.Utility.FilteSqlInfusion(context.Request["Content"]).Replace('|','\n').Replace("#","+");
        }

        string LotteryID = "";

        if (!string.IsNullOrEmpty(context.Request["LotteryID"]))
        {
            LotteryID = Shove._Web.Utility.FilteSqlInfusion(context.Request["LotteryID"]);
        }

        int PlayTypeID = 0;

        if (!string.IsNullOrEmpty(context.Request["PlayTypeID"]))
        {
            PlayTypeID = Shove._Convert.StrToInt(Shove._Web.Utility.FilteSqlInfusion(context.Request["PlayTypeID"]), 0);
        }

        if (string.IsNullOrEmpty(Content) || string.IsNullOrEmpty(LotteryID) || PlayTypeID == 0)
        {
            context.Response.Write("{\"message\": \"参数错误，请重新发起请求！\"");
            context.Response.End();
        }

        string Result = new SLS.Lottery()[Shove._Convert.StrToInt(LotteryID, 0)].AnalyseScheme(Content, PlayTypeID);

        context.Response.Write("{\"message\": \"1\" , \"Result\" : \"" + Result.Trim().Replace('\n', '#') + "\"}");
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}