<%@ WebHandler Language="C#" Class="getServerTime" %>

using System;
using System.Web;

public class getServerTime : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}