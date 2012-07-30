using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

using Shove.Database;

/// <summary>
/// Task 的摘要说明
/// </summary>
public class SendEmailTask
{
    private Sites Site;
    private string MailTo, Subject, Body;

    public string EmailServer_From = "";
    public string EmailServer_EmailServer = "";
    public string EmailServer_User = "";
    public string EmailServer_Password = "";

    private System.Threading.Thread thread;

    public SendEmailTask(Sites site, string mailto, string subject, string body)
    {
        Site = site;

        MailTo = mailto;
        Subject = subject;
        Body = body;
    }

    public void Run()
    {
        if ((Site == null) || (Site.ID < 1))
        {
            return;
        }

        if ((MailTo == "") || (Subject == "") || (Body == ""))
        {
            new Log("System").Write("Send Email: MailTo, Subject or Body parameters error.");

            return;
        }

        EmailServer_From = Site.SiteOptions["Opt_EmailServer_From"].Value.ToString();
        EmailServer_EmailServer = Site.SiteOptions["Opt_EmailServer_EmailServer"].Value.ToString();
        EmailServer_User = Site.SiteOptions["Opt_EmailServer_UserName"].Value.ToString();
        EmailServer_Password = Site.SiteOptions["Opt_EmailServer_Password"].Value.ToString();

        if ((EmailServer_From == "") || (EmailServer_EmailServer == "") || (EmailServer_User == ""))
        {
            new Log("System").Write("Send Email: Read EmailServer configure fail.");

            return;
        }

        lock (this) // 确保临界区被一个 Thread 所占用
        {
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
            thread.IsBackground = true;

            thread.Start();
        }
    }

    public void Do()
    {
        System.Threading.Thread.Sleep(2000);

        int TryNum = 0;
        int Result = -1;

        while ((Result < 0) && (TryNum++ < 3))
        {
            Result = Shove._Net.Email.SendEmail(EmailServer_From, MailTo, Subject, Body, EmailServer_EmailServer, EmailServer_User, EmailServer_Password);
        }

        if (Result < 0)
        {
            new Log("System").Write("Send Email: Send Mail fail. Tryed: " + TryNum.ToString());
        }

        Stop();
    }

    private void Stop()
    {
        //if (thread != null)
        //{
        //    thread.Abort();
        //    thread = null;
        //}
    }
}
