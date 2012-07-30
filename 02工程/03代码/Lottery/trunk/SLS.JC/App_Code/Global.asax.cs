using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Timers;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using Shove.Database;

public class Global : System.Web.HttpApplication
{
    /// <summary>
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public Global()
    {
        InitializeComponent();
    }

    protected void Application_Start(Object sender, EventArgs e)
    {
    }

    protected void Session_Start(Object sender, EventArgs e)
    {
        this.Session["SessionStart"] = 1;
    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {

    }

    protected void Application_EndRequest(Object sender, EventArgs e)
    {

    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {

    }

    protected void Application_Error(Object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (objErr == null)
        {
            Server.ClearError();

            return;
        }

        string Url = "空";
        try
        {
            Url = HttpContext.Current.Request.Url.ToString();
        }
        catch { }

        string ErrorMsg = "Error, PageName: " + Url + "。 ErrorMsg: " + objErr.Message + " Source:" + objErr.Source + " StackTrace:" + objErr.StackTrace + "。";

        new Log("System").Write(ErrorMsg);
    }

    protected void Session_End(Object sender, EventArgs e)
    {

    }

    protected void Application_End(Object sender, EventArgs e)
    {

    }

    public override string GetVaryByCustomString(HttpContext context, string custom)
    {
        if (custom == "SitePage")
        {
            return Shove._Web.Utility.GetUrlWithoutHttp();
        }

        return base.GetVaryByCustomString(context, custom);
    }

    #region Web 窗体设计器生成的代码
    /// <summary>
    /// 设计器支持所需的方法 - 不要使用代码编辑器修改
    /// 此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
    }
    #endregion
}
