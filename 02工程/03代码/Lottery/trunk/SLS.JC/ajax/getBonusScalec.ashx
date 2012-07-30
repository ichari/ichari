<%@ WebHandler Language="C#" Class="getBonusScalec" %>

using System;
using System.Web;
using System.Data;

public class getBonusScalec : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        //获得站点选项中的佣金比率
        DataTable dt = new DAL.Tables.T_Sites().Open("Opt_InitiateSchemeBonusScale,Opt_InitiateSchemeLimitLowerScaleMoney,Opt_InitiateSchemeLimitLowerScale,Opt_InitiateSchemeLimitUpperScaleMoney,Opt_InitiateSchemeLimitUpperScale", "", "");
        
        //把佣金比率换成整数
        string bScalec = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeBonusScale"].ToString(), 0) * 100).ToString();

        //发起方案条件
        string Opt_InitiateSchemeLimitLowerScaleMoney = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitLowerScaleMoney"].ToString(), 100)).ToString();
        string Opt_InitiateSchemeLimitLowerScale = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitLowerScale"].ToString(), 0.2)).ToString();
        string Opt_InitiateSchemeLimitUpperScaleMoney = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitUpperScaleMoney"].ToString(), 10000)).ToString();
        string Opt_InitiateSchemeLimitUpperScale = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitUpperScale"].ToString(), 0.05)).ToString();
        
        context.Response.Write("{\"bScalec\": \"" + bScalec + "\",\"LScaleMoney\": \"" + Opt_InitiateSchemeLimitLowerScaleMoney + "\",\"LScale\": \"" + Opt_InitiateSchemeLimitLowerScale + "\" ,\"UScaleMoney\": \"" + Opt_InitiateSchemeLimitUpperScaleMoney + "\", \"UpperScale\": \"" + Opt_InitiateSchemeLimitUpperScale + "\" }");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}