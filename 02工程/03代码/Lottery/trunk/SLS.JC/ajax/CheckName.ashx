<%@ WebHandler Language="C#" Class="CheckName" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

public class CheckName : IHttpHandler
{
    public void ProcessRequest (HttpContext context) {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";

        string UserName = "";

        if (string.IsNullOrEmpty(context.Request["UserName"]))
        {
            context.Response.Write("{\"message\": \"-1\"}");
            context.Response.End();
        }

        UserName = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "UserName"));

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(UserName) + "'", "");

        if ((dt != null && dt.Rows.Count > 0) || !IsKeyWords(UserName))
        {
            string UserName1 = UserName + DateTime.Now.Year.ToString();
            string UserName2 = UserName + DateTime.Now.Month.ToString();
            string UserName3 = UserName + DateTime.Now.Day.ToString();

            while (true)
            {
                dt = new DAL.Tables.T_Users().Open("ID", "Name ='" + UserName1 + "' or Name='" + UserName2 + "' or Name='" + UserName3 + "'", "");

                if (dt.Rows.Count < 1 && IsKeyWords(UserName1))
                {
                    break;
                }

                if (!IsKeyWords(UserName1))
                {
                    UserName = RandomNumber() + RandomNumber() + RandomNumber() + RandomNumber();
                }

                UserName1 = UserName + RandomNumber();
                UserName2 = UserName + RandomNumber() + DateTime.Now.Year.ToString();
                UserName3 = UserName + RandomNumber() + DateTime.Now.Month.ToString();
            }

            context.Response.Write("{\"message\": \"-2\", \"UserName1\": \"" + UserName1 + "\", \"UserName2\": \"" + UserName2 + "\", \"UserName3\": \"" + UserName3 + "\"}");
            context.Response.End();
        }

        if (Shove._String.GetLength(UserName) < 4 || Shove._String.GetLength(UserName) > 16)
        {
            context.Response.Write("{\"message\": \"-3\"}");
            context.Response.End();
        }
        
        context.Response.Write("{\"message\": \"0\"}");
        context.Response.End();
    }

    private bool IsKeyWords(string UserName)
    {
        DataTable dtKeyWords = new DAL.Tables.T_Sensitivekeywords().Open("", "", "");

        string KeyWords = "";

        if (dtKeyWords == null || dtKeyWords.Rows.Count < 1)
        {
            return true;
        }

        KeyWords = dtKeyWords.Rows[0]["KeyWords"].ToString().Replace("<p>", "").Replace("</p>", "");

        foreach (string str in KeyWords.Split(','))
        {
            if (UserName.IndexOf(str) >= 0)
            {
                return false;
            }
        }

        return true;
    }

    private string RandomNumber()
    {
        int Number = 0;
        string Str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        for (int i = 0; i < 1; i++)
        {
            Random rd = new Random();　　//无参即为使用系统时钟为种子
            Number += rd.Next(0, 61);
        }

        System.Threading.Thread.Sleep(100);

        return Str.Substring(Number, 1);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}