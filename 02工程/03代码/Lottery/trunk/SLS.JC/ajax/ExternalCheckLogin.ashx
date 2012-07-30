<%@ WebHandler Language="C#" Class="ExternalCheckLogin" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

public class ExternalCheckLogin : IHttpHandler
{
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
            context.Response.Write(context.Request.QueryString["jsoncallback"] + "({\"message\": \"-1\",\"name\": \"\",\"Balance\": \"0\"})");

            return;
        }

        context.Response.Write(context.Request.QueryString["jsoncallback"] + "({\"message\": \"" + _User.ID.ToString() + "\",\"name\": \"" + _User.Name + "\",\"Balance\": \"" + _User.Balance.ToString() + "\"})");
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}